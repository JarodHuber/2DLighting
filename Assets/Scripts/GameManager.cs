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

        cam.target = player;

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

        if (!cam.peek)
        {
            cam.target = player;
            cam.peek = true;
        }

        globalLight.color = globalLightColor;
        Invoke("ShowLivesDelayed", showLivesDelay);
    }

    void ShowLivesDelayed()
    {
        livesCounter.ToggleUI();
    }

    public void TogglePlayerPause()
    {
        player.TogglePause();
        cam.peek = !cam.peek;
    }
}
