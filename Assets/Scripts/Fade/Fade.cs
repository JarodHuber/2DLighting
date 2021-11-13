using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{
    [SerializeField] FadeManager fManager = null;
    [SerializeField] FadeType fadeType;
    public UnityEvent broadcastFadeComplete;

    public void HandleFade()
    {
        fManager.BeginFade(fadeType, this);
    }
}
