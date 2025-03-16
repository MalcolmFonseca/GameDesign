using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float speed = 2f;

    public virtual void Move() { }
    public virtual void Attack() { }
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    protected void FlipSprite() 
    {
        
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

}
