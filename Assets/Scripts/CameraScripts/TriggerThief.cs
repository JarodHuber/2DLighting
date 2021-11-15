using UnityEngine;

public class TriggerThief : MonoBehaviour, ICameraHolder
{
    [SerializeField] CameraThief thief = null;
    [SerializeField] Vector3 cameraOffset = new Vector3(0.0f, 0.0f, 5.0f);

    public Vector3 CameraOffset()
    {
        return cameraOffset;
    }

    public Vector3 Position()
    {
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            thief.StealCamera(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            thief.ReturnCamera();
    }
}
