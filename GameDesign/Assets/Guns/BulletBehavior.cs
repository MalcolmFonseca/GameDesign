using UnityEngine;

public abstract class BulletBehavior : MonoBehaviour
{
    public float bulletSpeed;
    public int bulletDamage;
    private float timer;

    public abstract void Start();

    public virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5) Destroy(gameObject); // Destroy after 5 seconds
    }

    // Allow derived classes to define what happens on collision
    public abstract void OnTriggerEnter2D(Collider2D other);

    
}
