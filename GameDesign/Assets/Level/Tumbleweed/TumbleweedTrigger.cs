using UnityEngine;

public class TumbleweedTrigger : MonoBehaviour
{
    public GameObject tumbleweedPrefab;
    public Transform spawnPoint;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!triggered & collision.CompareTag("Player")) {
            triggered = true;
            Instantiate(tumbleweedPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
