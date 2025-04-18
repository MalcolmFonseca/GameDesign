using UnityEngine;
using UnityEngine.SceneManagement;

public class UndergroundEntrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponentInChildren<Player>();
            if (player != null)
            {
                SceneManager.LoadScene("Underground");
            }
        }
    }
}
