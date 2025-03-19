using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed;
    public int bulletDamage;

    private GameObject player;
    private Rigidbody2D rb;
    private float timer;

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
        timer += Time.deltaTime;
        
        if (timer > 5) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.TakeDamage(bulletDamage);

            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Ground")) Destroy(gameObject); 

    }
}
