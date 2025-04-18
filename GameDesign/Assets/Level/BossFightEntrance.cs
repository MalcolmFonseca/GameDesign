using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFightEntrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponentInChildren<Player>();
            if (player != null)
            {
                SceneManager.LoadScene("BossFight");
            }
        }
    }
}

