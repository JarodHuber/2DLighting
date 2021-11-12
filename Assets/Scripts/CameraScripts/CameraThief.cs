using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThief : MonoBehaviour
{
    [SerializeField] CameraFollow toSteal = null;

    ICameraHolder stolenFrom = null;
    bool wasUsingPeekBeforeTheft = false;

    public void StealCamera(ICameraHolder toGive)
    {
        stolenFrom = toSteal.target;
        toSteal.target = toGive;
        wasUsingPeekBeforeTheft = toSteal.peek;
        toSteal.peek = false;
    }

    public void ReturnCamera()
    {
        toSteal.target = stolenFrom;
        toSteal.peek = wasUsingPeekBeforeTheft;
    }
}