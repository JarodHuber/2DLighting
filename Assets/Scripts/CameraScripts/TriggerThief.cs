using UnityEngine;

public class TriggerThief : MonoBehaviour
{
    [SerializeField] CameraThief thief = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            thief.StealCamera();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            thief.ReturnCamera();
    }
}
