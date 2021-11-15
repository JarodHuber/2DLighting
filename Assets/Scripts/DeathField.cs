using UnityEngine;

public class DeathField : MonoBehaviour
{
    [SerializeField]
    private GameManager manager = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            manager.Respawn();
    }
}
