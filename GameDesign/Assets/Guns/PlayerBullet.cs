using UnityEngine;

public class PlayerBullet : BulletBehavior
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.right * bulletSpeed;

        // bullet rotation
        float rotation = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);
    }


    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
            if (enemy != null) enemy.TakeDamage(bulletDamage);

            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Ground")) Destroy(gameObject);
    }
}
