using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public MovementController motor = null;
    [SerializeField] Light2D areaLight = null;
    [SerializeField] Counter lives = new Counter(3);
    [SerializeField] TMP_Text livesUI = null;

    Vector3 spawn = new Vector3();
    Color areaLightColor = new Color();

    private void Start()
    {
        spawn = transform.position;
        areaLightColor = areaLight.color;
        livesUI.text = lives.AmountRemaining.ToString();
    }

    public bool Respawn()
    {
        if (lives.PreCheck())
            return false;

        transform.position = spawn;
        areaLight.color = areaLightColor;
        livesUI.text = lives.AmountRemaining.ToString();

        return true;
    }

    public void TogglePause()
    {
        motor.isPaused = !motor.isPaused;
    }
}
