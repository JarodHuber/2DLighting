using UnityEngine;
using UnityEngine.Events;

public class FadeCall : SystemCaller
{
    [SerializeField]
    private FadeManager fManager = null;
    [SerializeField]
    private FadeType fadeType;
    [SerializeField]
    private float fadeDur = 1.0f;

    [Space(10)]
    public UnityEvent broadcastFadeComplete;

    public override void Broadcast()
    {
        fManager.BeginFade(fadeType, this, fadeDur);
    }

    public override void Callback()
    {
        broadcastFadeComplete.Invoke();
    }
}
