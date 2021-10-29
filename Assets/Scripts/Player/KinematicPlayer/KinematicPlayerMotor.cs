using UnityEngine;

/// <summary>
/// Basic kinematic player motor demonstrating how to implement a motor for a KinematicBody
/// </summary>
public class KinematicPlayerMotor : MonoBehaviour, IKinematicMotor
{
    [Header("Body")]
    public KinematicBody body;
    
    [Header("Common Movement Settings")]
    public float moveSpeed = 8.0f;
    public float jumpHeight = 2.0f;
    
    [Header("Ground Movement")]
    public float maxGroundAngle = 75f;
    public float groundAccel = 200.0f;
    public float groundFriction = 12.0f;
    public LayerMask groundLayers;
    public float maxGroundAdhesionDistance = 0.1f;
    
    public bool Grounded { get; private set; }
    private bool wasGrounded;
    
    [Header("Air Movement")]
    public float airAccel = 50.0f;
    public float airFriction = 3.0f;

    private bool jumpedThisFrame;

    // Input handling
    private Vector2 moveWish;
    private bool jumpWish;

    //
    // Motor API
    //

    public void MoveInput(Vector2 move)
    {
        moveWish = move;
    }

    public void JumpInput()
    {
        jumpWish = true;
    }
    public bool JumpTrigger()
    {
        return jumpedThisFrame;
    }
    
    //
    // Motor Utilities
    //
    
    public Vector2 ClipVelocity(Vector2 inputVelocity, Vector2 normal)
    {
        return inputVelocity - (normal * Vector2.Dot(inputVelocity, normal));
    }

    //
    // IKinematicMotor implementation
    //

    public Vector2 UpdateVelocity(Vector2 oldVelocity)
    {
        Vector2 velocity = oldVelocity;
        
        //
        // integrate player forces
        //
        
        if (jumpWish)
        {
            jumpWish = false;

            if(wasGrounded)
            {
                jumpedThisFrame = true;

                velocity.y += Mathf.Sqrt(-2.0f * body.EffectiveGravity.y * jumpHeight);
            }
        }
        
        bool isGrounded = !jumpedThisFrame && wasGrounded;

        float effectiveAccel = (isGrounded ? groundAccel : airAccel);
        float effectiveFriction = (isGrounded ? groundFriction : airFriction);
        
        // apply friction
        float keepY = velocity.y;
        velocity.y = 0.0f; // don't consider vertical movement in friction calculation
        float prevSpeed = velocity.magnitude;
        if (prevSpeed != 0)
        {
            float frictionAccel = prevSpeed * effectiveFriction * Time.deltaTime;
            velocity *= Mathf.Max(prevSpeed - frictionAccel, 0) / prevSpeed;
        }

        velocity.y = keepY;

        // apply movement
        moveWish = Vector2.ClampMagnitude(moveWish, 1);
        float velocityProj = Vector2.Dot(velocity, moveWish);
        float accelMag = effectiveAccel * Time.deltaTime;

        // clamp projection onto movement vector
        if (velocityProj + accelMag > moveSpeed)
        {
            accelMag = moveSpeed - velocityProj;
        }

        return velocity + (moveWish * accelMag);
    }

    public void OnMoveHit(ref Vector2 curPosition, ref Vector2 curVelocity, Collider2D other, Vector2 direction, float pen)
    {
        Vector2 clipped = ClipVelocity(curVelocity, direction);
        
        // floor
        if (groundLayers.Test(other.gameObject.layer) &&  // require ground layer
            direction.y > 0 &&                                      // direction check
            Vector2.Angle(direction, Vector2.up) < maxGroundAngle)  // angle check
        {
            // only change Y-position if bumping into the floor
            curPosition.y += direction.y * (pen);
            curVelocity.y = clipped.y;
            
            Grounded = true;
        }
        // other
        else
        {
            curPosition += direction * (pen);
            curVelocity = clipped;
        }
    }

    public void OnPreMove()
    {
        // reset frame data
        jumpedThisFrame = false;
        Grounded = false;
    }
    public void OnFinishMove(ref Vector2 curPosition, ref Vector2 curVelocity)
    {
        // Ground Adhesion

        // early exit if we're already grounded or jumping
        if (Grounded || jumpedThisFrame || !wasGrounded) return;
        
        var groundCandidates = body.Cast(curPosition, Vector3.down, maxGroundAdhesionDistance, groundLayers);
        foreach (var candidate in groundCandidates)
        {
            // ignore colliders that we start inside of - it's either us or something bad happened
            if(candidate.point == Vector2.zero) { continue; }

            // NOTE: This code assumes that the ground will always be below us
            curPosition.y = candidate.point.y - body.FootOffset.y - body.contactOffset;
            
            // Snap to the ground - perform any necessary collision and sliding logic
            body.DeferredCollideAndSlide(ref curPosition, ref curVelocity, candidate.collider);
            //Debug.Assert(snapPosition.y >= candidate.point.y, "Snapping put us underneath the ground?!");
            break;
        }
    }

    public void OnPostMove()
    {
        // record grounded status for next frame
        wasGrounded = Grounded;
    }
    
    //
    // Unity Messages
    //

    private void Start()
    {
        body.motor = this;
        OnValidate();
    }
    
    private void OnValidate()
    {
        if(body == null ||body.BodyCollider == null) { return; }
    }
}
