using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [System.Flags]
    enum PeekBools
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
    }

    public ICameraHolder target;
    [SerializeField] float smoothSpeed = 0.2f;
    [SerializeField] 
    Vector2 min = new Vector2(-5.0f, -5.0f), 
            max = new Vector2(5.0f, 5.0f);
    [Tooltip("Offset for positional z should always be -10.0f")]
    [SerializeField] float cameraZOffset = -10.0f;

    [Header("Peek Variables")]
    [SerializeField] PeekBools peekAxes = (PeekBools)(-1);
    [SerializeField] Vector3 peekPower = new Vector3(1.5f, 3.0f);
    
    [HideInInspector]
    public bool peek = true;

    Vector3 currentVelocity = new Vector3();

    private void FixedUpdate()
    {
        // Initialize targetPos
        Vector3 targetPos = target.Position();

        // Calculate peek if any
        if (peek)
        {
            Vector3 peekOffset = new Vector3(
                (peekAxes & PeekBools.Horizontal) != PeekBools.None ? Input.GetAxisRaw("Horizontal") : 0,
                (peekAxes & PeekBools.Vertical) != PeekBools.None ? Input.GetAxisRaw("Vertical") : 0,
                0);
            peekOffset.Scale(peekPower);

            targetPos += peekOffset;
        }
        // Clamp targetPos and apply Offset
        targetPos = targetPos.Clamp(min, max) + target.CameraOffset();

        // Combine positional and Orthographic size data
        Vector3 preData = transform.position;
        preData.z = Camera.main.orthographicSize;

        // Calculate SmoothDamp for data
        Vector3 translateData = Vector3.SmoothDamp(preData, targetPos, ref currentVelocity, smoothSpeed);

        // Apply Orthographic and positional data
        Camera.main.orthographicSize = translateData.z;

        translateData.z = cameraZOffset;
        transform.position = translateData;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 bounds = max - min;
        Gizmos.DrawWireCube(min + bounds / 2, bounds);
    }
}

public interface ICameraHolder
{
    public Vector3 CameraOffset();
    public Vector3 Position();
}