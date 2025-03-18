using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float speed = 2f;

    public virtual void Move() { }

    public virtual void ChasePlayer() { }

    public virtual void Patrol() { }


    public virtual void Attack() { }
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    protected bool FlipSprite(float direction, bool isFacingRight) 
    {
        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            return !isFacingRight;
        }

        return isFacingRight;
        



    }

    protected void Die()
    {
        Destroy(gameObject);
    }

}
