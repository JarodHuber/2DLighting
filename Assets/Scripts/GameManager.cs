using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController player = null;
    [SerializeField] CameraFollow cam = null;
    [SerializeField] Light2D globalLight = null;
    [SerializeField] UISlide livesCounter = null;
    [SerializeField] Timer livesVisibleDur = new Timer(3.0f);
    [SerializeField] float showLivesDelay = 1.0f;

    Color globalLightColor = new Color();

    private void Start()
    {
        globalLightColor = globalLight.color;

        livesCounter.ToggleUI();
    }

    private void Update()
    {
        if (!livesCounter.visible || livesCounter.sliding)
            return;

        if (livesVisibleDur.Check())
            livesCounter.ToggleUI();
    }

    public void Restart()
    {
        if(!player.Respawn())
        {
            print("player died");
            return;
        }

        if (!cam.followPlayer)
        {
            cam.target = player.transform;
            cam.followPlayer = true;
        }

        globalLight.color = globalLightColor;
        Invoke("ShowLivesDelayed", showLivesDelay);
    }

    void ShowLivesDelayed()
    {
        livesCounter.ToggleUI();
    }
}
