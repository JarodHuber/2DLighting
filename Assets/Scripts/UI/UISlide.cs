using UnityEngine;

public class UISlide : MonoBehaviour
{
    public bool visible = false;
    public bool sliding = false;
    [SerializeField] Vector3 hiddenPos = new Vector3();
    [SerializeField] Vector3 visiblePos = new Vector3();
    [SerializeField] Timer slideDur = new Timer(1.0f);
    [SerializeField] RectTransform slider = null;

    private void Update()
    {
        if (!sliding)
            return;

        if (slideDur.Check(false))
            sliding = false;

        if (visible)
            slider.anchoredPosition = Vector3.Lerp(hiddenPos, visiblePos, slideDur.PercentComplete);
        else
            slider.anchoredPosition = Vector3.Lerp(visiblePos, hiddenPos, slideDur.PercentComplete);
    }

    public void ToggleUI()
    {
        visible = !visible;

        if (sliding)
            slideDur.Reset(slideDur.TimeRemaining);
        else
            slideDur.Reset();

        sliding = true;
    }
}
