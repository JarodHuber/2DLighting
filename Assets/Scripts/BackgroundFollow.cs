using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] Vector3 offset = new Vector3(0, 0, 10);
    [SerializeField] SpriteRenderer sprite = null;

    private void Update()
    {
        transform.position = target.position + offset;

        float scaler = (Camera.main.orthographicSize * 2);
        Vector3 scale;

        if (sprite) 
            scale = new Vector3(scaler / sprite.size.y, scaler / sprite.size.y, 1);
        else
            scale = new Vector3(Camera.main.aspect * scaler, scaler, 1);

        transform.localScale = scale;
    }
}

// TODO: Add Trigger script that swaps Sky and enables shadows on sky