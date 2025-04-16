using UnityEngine;

public class BottomBoundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponentInChildren<Player>();
            if (player != null)
            {
                player.Respawn();
            }
        }
    }
}
