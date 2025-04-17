using System.Buffers.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 bossOffset = new Vector3(0f, 0f, 0f);

    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 camPos = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -10
        );

        if (SceneManager.GetActiveScene().name == "BossFight")
        {
            camPos += bossOffset;
        }


        transform.position = camPos;
    }
}
