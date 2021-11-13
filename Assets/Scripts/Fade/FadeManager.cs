using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NaughtyAttributes;

public enum FadeType
{
    FadeIn,
    FadeOut,
    FullFade
}

public class FadeManager : MonoBehaviour
{
    [SerializeField] 
    private Image fadeImage = null;
    [SerializeField] 
    private Timer fadeTime = new Timer(1);
    [SerializeField, ReadOnly]
    private FadeType fadeType = FadeType.FadeIn;

    float startingOpacity, targetOpacity;
    bool isFading = false;
    Fade caller = null;

    public void BeginFade(FadeType fade, Fade fadeCaller)
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

        caller = fadeCaller;
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
                startingOpacity = 0.0f;
                targetOpacity = 1.0f;
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

        isFading = false;
        caller.broadcastFadeComplete.Invoke();
        caller = null;
    }
}
