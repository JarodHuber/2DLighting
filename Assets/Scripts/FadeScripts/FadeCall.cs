using UnityEngine;
using UnityEngine.Events;

public class FadeCall : MonoBehaviour, ICaller
{
    [SerializeField]
    private FadeManager fManager = null;
    [SerializeField]
    private FadeType fadeType;
    [SerializeField]
    private float fadeDur = 1.0f;

    [Space(10)]
    public UnityEvent broadcastFadeComplete;

    public void HandleFade()
    {
        fManager.BeginFade(fadeType, this, fadeDur);
    }

    public void Callback()
    {
        broadcastFadeComplete.Invoke();
    }
}
