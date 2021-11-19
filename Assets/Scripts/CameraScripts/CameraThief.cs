using UnityEngine;

public class CameraThief : MonoBehaviour, ICameraHolder
{
    [SerializeField] CameraFollow toSteal = null;
    [SerializeField] Vector3 cameraOffset = new Vector3(0.0f, 0.0f, 5.0f);

    ICameraHolder stolenFrom = null;
    bool wasUsingPeekBeforeTheft = false;

    public Vector3 CameraOffset()
    {
        return cameraOffset;
    }
    public Vector3 Position()
    {
        return transform.position;
    }

    public void StealCamera()
    {
        stolenFrom = toSteal.target;
        toSteal.target = this;
        wasUsingPeekBeforeTheft = toSteal.peek;
        toSteal.peek = false;
    }
    public void ReturnCamera()
    {
        toSteal.target = stolenFrom;
        toSteal.peek = wasUsingPeekBeforeTheft;
    }
}