using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightTrigger : MonoBehaviour
{
    [SerializeField] 
    private Light2D effectedLight = null;

    [SerializeField] 
    private Transform start = null, end = null;
    [SerializeField]
    private Color startC = new Color();
    [SerializeField]
    private float startIntensity = 1.0f;
    [SerializeField]
    private Color endC = new Color();
    [SerializeField]
    private float endIntensity = 1.0f;

    Transform target = null;
    bool lerping = false;

    private void Update()
    {
        if (!lerping)
            return;

        float u = Vector3.Dot(start.position - target.position, start.position - end.position);
        u /= Vector3.Dot(start.position - end.position, start.position - end.position);

        u = Mathf.Clamp(u, 0.0f, 1.0f);

        effectedLight.color = Color.Lerp(startC, endC, u);
        effectedLight.intensity = Mathf.Lerp(startIntensity, endIntensity, u);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lerping = true;
            target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.transform)
        {
            lerping = false;
            target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(start.position, end.position);
    }
}
