using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour, ICameraHolder
{
    public MovementController motor = null;
    [SerializeField]
    private Light2D areaLight = null;
    [SerializeField]
    private Counter lives = new Counter(3);
    [SerializeField]
    private TMP_Text livesUI = null;

    [Space(10)]
    public Vector3 cameraOffset = new Vector3(0, 2.0f, 5.0f);

    private Vector3 spawn = new Vector3();
    private Color areaLightColor = new Color();

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

    public Vector3 Position()
    {
        return transform.position;
    }

    public Vector3 CameraOffset()
    {
        return cameraOffset;
    }
}
