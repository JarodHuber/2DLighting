using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] Vector3 offset = new Vector3(0, 0, 10);
    [SerializeField] SpriteRenderer[] backdrops = new SpriteRenderer[0];
    public int targetBackdrop = 0;

    SpriteRenderer Sprite => backdrops[targetBackdrop];

    private void Start()
    {
        if (backdrops.Length != 0)
            return;

        backdrops = new SpriteRenderer[transform.childCount];

        for (int x = 0; x < backdrops.Length; ++x)
            backdrops[x] = transform.GetChild(x).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position = target.position + offset;

        if (!Sprite)
            throw new System.ArgumentNullException();

        float scaler = (Camera.main.orthographicSize * 2);
        Vector3 scale = new Vector3(Camera.main.aspect * scaler, scaler, 1);

        if (Sprite.drawMode == SpriteDrawMode.Tiled)
            Sprite.size = scale / Sprite.transform.localScale.y;
        else
            Sprite.transform.localScale = scale;
    }
}