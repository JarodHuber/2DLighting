using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType
{
    FadeIn,
    FadeOut,
    FullFade
}

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private GameManager manager = null;
    [SerializeField]
    private Image fadeImage = null;

    [Space(10)]
    [SerializeField, ReadOnly]
    private FadeType fadeType = FadeType.FadeIn;
    [SerializeField, ReadOnly]
    private Timer fadeTime = new Timer(1);

    private float startingOpacity, targetOpacity;
    private bool isFading = false;
    private SystemCaller caller = null;

    public void BeginFade(FadeType fade, FadeCall fadeCaller, float fadeDur)
    {
        fadeType = fade;
        isFading = true;
        fadeTime.SetMax(fadeDur, true);

        switch (fadeType)
        {
            case FadeType.FullFade:
            case FadeType.FadeOut:
                startingOpacity = 0.0f;
                targetOpacity = 1.0f;
                break;
            case FadeType.FadeIn:
                startingOpacity = 1.0f;
                targetOpacity = 0.0f;
                break;
        }

        caller = fadeCaller;
        manager.TogglePlayerPause();
    }
    private void BeginFade(FadeType fade)
    {
        fadeType = fade;
        isFading = true;

        switch (fadeType)
        {
            case FadeType.FullFade:
            case FadeType.FadeOut:
                startingOpacity = 0.0f;
                targetOpacity = 1.0f;
                break;
            case FadeType.FadeIn:
                startingOpacity = 1.0f;
                targetOpacity = 0.0f;
                break;
        }
    }

    private void Update()
    {
        if (isFading)
            PerformFade();
    }

    private void PerformFade()
    {
        Color tmpColor = fadeImage.color;

        if (fadeTime.Check())
        {
            tmpColor.a = targetOpacity;
            fadeImage.color = tmpColor;
            HandleFadeComplete();
            return;
        }

        tmpColor.a = Mathf.Lerp(startingOpacity, targetOpacity, fadeTime.PercentComplete);
        fadeImage.color = tmpColor;
    }

    private void HandleFadeComplete()
    {
        if (fadeType == FadeType.FullFade)
        {
            BeginFade(FadeType.FadeIn);
            return;
        }

        manager.TogglePlayerPause();

        isFading = false;
        caller.Callback();
    }
}
