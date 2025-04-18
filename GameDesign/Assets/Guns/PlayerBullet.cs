using UnityEngine;

public class PlayerBullet : BulletBehavior
{
    private Rigidbody2D rb;
    private Gun gun;
    public bool special = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = transform.right * bulletSpeed;

        gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();

        // bullet rotation
        float rotation = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);
    }


    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<EnemyAI>(out var enemy))
        {
            if (special)
            {
                enemy.TakeDamage((int)(gun.bulletDamage*1.2));
            } else
            {
                enemy.TakeDamage(gun.bulletDamage);
            }
            Destroy(gameObject);
            return;
        }

        if (other.TryGetComponent<CrowBossAI>(out var boss))
        {
            boss.TakeDamage(gun.bulletDamage);
            Destroy(gameObject);
            return;
        }

        else if (other.gameObject.CompareTag("Ground")) Destroy(gameObject);
    }
}
