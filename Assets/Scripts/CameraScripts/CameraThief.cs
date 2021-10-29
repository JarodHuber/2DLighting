using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThief : MonoBehaviour
{
    [SerializeField] CameraFollow toSteal = null;

    Transform stolenFrom = null;
    bool followPlayer = false;

    public void StealCamera(Transform toGive)
    {
        stolenFrom = toSteal.target;
        toSteal.target = toGive;
        followPlayer = toSteal.followPlayer;
        toSteal.followPlayer = false;
    }

    public void ReturnCamera()
    {
        toSteal.target = stolenFrom;
        toSteal.followPlayer = followPlayer;
    }
}
