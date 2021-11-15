using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// A fully-kinematic body with callbacks for handling collision and movement
/// </summary>
public class KinematicBody : MonoBehaviour
{
    /// <summary>
    /// Motor driving this body
    /// </summary>
    public IKinematicMotor motor;

    [Header("Body Definition")]
#pragma warning disable 0649 // Assigned in Unity inspector
    [SerializeField]
    private BoxCollider2D col;
    public BoxCollider2D BodyCollider => col;
    [SerializeField]
    private Rigidbody2D rbody;
    [SerializeField]
    private bool useGravity = true;
#pragma warning restore 0649 // Assigned in Unity inspector
    /// <summary>
    /// Size of the box body in local space
    /// </summary>
    public Vector2 LocalBodySize => col.size;
    /// <summary>
    /// Minimum desired distance between nearby surfaces and the surface of this body
    /// </summary>
    [FormerlySerializedAs("skinWidth")] public float contactOffset = 0.005f;
    /// <summary>
    /// Size of the box body in local space inclusive of the contact offset
    /// </summary>
    public Vector2 LocalBodySizeWithSkin => col.size + Vector2.one * contactOffset;
    public Vector2 GetLocalOffsetToCenter()
    {
        return col.offset;
    }
    public Vector2 GetCenterAtBodyPosition(Vector2 bodyPosition)
    {
        return bodyPosition + col.offset;
    }
    /// <summary>
    /// Position of the feet (aka bottom) of the body
    /// </summary>
    public Vector3 FootPosition => transform.TransformPoint(col.offset + Vector2.down * col.size.y / 2.0f);
    /// <summary>
    /// Offset from the pivot of the body to the feet
    /// </summary>
    public Vector2 FootOffset => (FootPosition - transform.position);

    [Header("Body Settings")]
    public Vector2 GravityScale = new Vector2(0, 2);
    // velocity of the final object inclusive of external forces, given in world-space
    public Vector2 EffectiveGravity
    {
        get
        {
            Vector2 g = Physics2D.gravity;
            g.Scale(GravityScale);
            return g;
        }
    }

    public Vector2 InternalVelocity { get; private set; }
    public Vector2 Velocity { get; private set; }

    LayerMask collisionMask = new LayerMask();

    public void CollideAndSlide(Vector2 bodyPosition, Vector2 bodyVelocity, Collider2D other)
    {
        DeferredCollideAndSlide(ref bodyPosition, ref bodyVelocity, other);

        // apply movement immediately
        rbody.MovePosition(bodyPosition);
        InternalVelocity = bodyVelocity;
    }

    public void DeferredCollideAndSlide(ref Vector2 bodyPosition, ref Vector2 bodyVelocity, Collider2D other)
    {
        // ignore self collision
        if (other == col || other.isTrigger) { return; }

        ColliderDistance2D collisionData = Physics2D.Distance(col, other);

        if (collisionData.isOverlapped)
        {
            // defer to motor to resolve hit
            motor.OnMoveHit(ref bodyPosition, ref bodyVelocity, other, -collisionData.normal, -collisionData.distance);
        }
    }

    public Collider2D[] Overlap(Vector2 bodyPosition, int layerMask = ~0, QueryTriggerInteraction queryMode = QueryTriggerInteraction.UseGlobal)
    {
        bodyPosition = GetCenterAtBodyPosition(bodyPosition);
        return Physics2D.OverlapBoxAll(bodyPosition, LocalBodySize, rbody.rotation, layerMask);
    }

    public Collider2D[] Overlap(Vector2 bodyPosition, Vector2 bodyHalfExtents, int layerMask = ~0, QueryTriggerInteraction queryMode = QueryTriggerInteraction.UseGlobal)
    {
        bodyPosition = GetCenterAtBodyPosition(bodyPosition);
        return Physics2D.OverlapBoxAll(bodyPosition, bodyHalfExtents, rbody.rotation, layerMask);
    }

    public RaycastHit2D[] Cast(Vector2 bodyPosition, Vector2 direction, float distance, int layerMask = ~0, QueryTriggerInteraction queryMode = QueryTriggerInteraction.UseGlobal)
    {
        bodyPosition = GetCenterAtBodyPosition(bodyPosition);
        var allHits = Physics2D.BoxCastAll(bodyPosition, LocalBodySizeWithSkin / 2, rbody.rotation, direction, distance, layerMask);

        // TODO: this is terribly inefficient and generates garbage, please optimize this
        List<RaycastHit2D> filteredhits = new List<RaycastHit2D>(allHits);
        filteredhits.RemoveAll(x => x.collider == col);
        return filteredhits.ToArray();
    }

    //
    // Unity Messages
    //

    private void Start()
    {
        for (int x = 0; x < 31; ++x)
        {
            collisionMask |= (1 << x) * (Physics.GetIgnoreLayerCollision(gameObject.layer, x) ? 0 : 1);
        }
        OnValidate();
    }

    private void FixedUpdate()
    {
        Vector2 startPosition = rbody.position;

        motor.OnPreMove();

        InternalVelocity = motor.UpdateVelocity(InternalVelocity);

        //
        // integrate external forces
        //

        // apply gravity (if enabled)
        if (useGravity)
        {
            InternalVelocity += EffectiveGravity * Time.deltaTime;
        }

        rbody.position += (InternalVelocity * Time.deltaTime);
        Vector2 projectedPos = rbody.position;
        Vector2 projectedVel = InternalVelocity;

        //
        // depenetrate from overlapping objects
        //

        Vector2 sizeOriginal = col.size;
        Vector2 sizeWithSkin = col.size + Vector2.one * contactOffset;

        var candidates = Overlap(projectedPos, sizeWithSkin, collisionMask);

        // HACK: since we can't pass a custom size to Physics.ComputePenetration (see below),
        //       we need to assign it directly to the collide prior to calling it and then
        //       revert the change afterwards
        col.size = sizeWithSkin;

        foreach (var candidate in candidates)
        {
            DeferredCollideAndSlide(ref projectedPos, ref projectedVel, candidate);
        }

        // HACK: restoring size (see above HACK)
        col.size = sizeOriginal;

        // callback: pre-processing move before applying 
        motor.OnFinishMove(ref projectedPos, ref projectedVel);

        // apply move
        rbody.MovePosition(projectedPos);
        InternalVelocity = projectedVel;

        Velocity = (projectedPos - startPosition) / Time.fixedDeltaTime;

        // callback for after move is complete
        motor.OnPostMove();
    }

    private void OnValidate()
    {
        contactOffset = Mathf.Clamp(contactOffset, 0.001f, float.PositiveInfinity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(col.offset, col.size + Vector2.one * contactOffset);
    }
}

public interface IKinematicMotor
{
    /// <summary>
    /// Called by KinematicBody when it initially updates its velocity
    /// </summary>
    /// <param name="oldVelocity">Existing velocity</param>
    /// <returns>Returns the new velocity to apply to the body</returns>
    Vector2 UpdateVelocity(Vector2 oldVelocity);

    /// <summary>
    /// Called by KinematicBody when the body hits another collider during its move
    /// </summary>
    /// <param name="curPosition">Position of the body at time of impact</param>
    /// <param name="curVelocity">Velocity of the body at time of impact</param>
    /// <param name="other">The collider that was struck</param>
    /// <param name="direction">Depenetration direction</param>
    /// <param name="pen">Penetration depth</param>
    void OnMoveHit(ref Vector2 curPosition, ref Vector2 curVelocity, Collider2D other, Vector2 direction, float pen);

    // TODO: Make these callbacks instead of part of the interface

    /// <summary>
    /// Called before the body has begun moving
    /// </summary>
    void OnPreMove();

    /// <summary>
    /// Called before the move is applied to the body.
    ///
    /// This provides an opportunity to perform any post-processing on the move
    /// before it is applied to the body.
    /// </summary>
    /// <param name="curPosition">Position that the body would move to</param>
    /// <param name="curVelocity">Velocity that the body would move with on next update</param>
    void OnFinishMove(ref Vector2 curPosition, ref Vector2 curVelocity);

    /// <summary>
    /// Called after the body has moved to its final position for this frame
    /// </summary>
    void OnPostMove();
}