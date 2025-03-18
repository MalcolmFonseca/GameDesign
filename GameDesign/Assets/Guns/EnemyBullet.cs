using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed;

    private GameObject player;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // bullet speed
        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;

        // bullet rotation
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
