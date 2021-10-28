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

    public Transform target = null;
    public Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] float smoothSpeed = 0.2f;
    [SerializeField] Vector3 min = new Vector3(-5.0f, -5.0f, 0.0f), max = new Vector3(5.0f, 5.0f, 0.0f);

    [Header("Peek Variables")]
    [SerializeField] PeekBools peekAxes = (PeekBools)(-1);
    [SerializeField] Vector3 peekPower = new Vector3(1.5f, 3.0f);

    Vector3 currentVelocity = new Vector3();

    private void FixedUpdate()
    {
        Vector3 peekOffset = new Vector3(
            (peekAxes & PeekBools.Horizontal) != PeekBools.None ? Input.GetAxisRaw("Horizontal") : 0, 
            (peekAxes & PeekBools.Vertical) != PeekBools.None ? Input.GetAxisRaw("Vertical") : 0, 
            0);
        peekOffset.Scale(peekPower);

        transform.position = Vector3.SmoothDamp(transform.position, (target.position + peekOffset).Clamp(min, max) + offset, ref currentVelocity, smoothSpeed);
    }
}
