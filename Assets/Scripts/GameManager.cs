using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player = null;
    [SerializeField]
    private CameraFollow cam = null;

    [Space(10)]
    [SerializeField]
    private Light2D globalLight = null;

    [Space(10)]
    [SerializeField]
    private UISlide livesCounter = null;
    [SerializeField]
    private Timer livesVisibleDur = new Timer(3.0f);
    [SerializeField]
    private float showLivesDelay = 1.0f;

    [Space(10)]
    [SerializeField]
    private FadeCall fadeIn = null;
    [SerializeField]
    private FadeCall fadeOut = null;

    private Color globalLightColor = new Color();

    private void Start()
    {
        globalLightColor = globalLight.color;

        cam.target = player;

        livesCounter.ToggleUI();

        fadeIn.Broadcast();
    }

    private void Update()
    {
        if (!livesCounter.visible || livesCounter.sliding)
            return;

        if (livesVisibleDur.Check())
            livesCounter.ToggleUI();
    }

    public void Respawn()
    {
        if (!player.Respawn())
        {
            print("player died");
            fadeOut.Broadcast();
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

        if ((Object)cam.target == player) 
            cam.peek = !player.IsPaused;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}