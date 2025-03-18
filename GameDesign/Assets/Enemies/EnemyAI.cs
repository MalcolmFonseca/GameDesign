using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    public Transform player;
    public float health = 100f;
    public float speed = 2f;
    public float detectionRange = 5f;


    
    protected EnemyShooting enemyShooting;


    public virtual void Move()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRange)
        {
            ChasePlayer();
            // Enable shooting when in range
            if (enemyShooting != null) enemyShooting.enabled = true;

        }

        else
        {
            // Disable shooting when out of range
            if (enemyShooting != null) enemyShooting.enabled = false;
            Patrol();
        }

    }

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
            // rotate method for flipping so bullets change direction accordingly
            transform.Rotate(0f, 180f, 0f);

            return !isFacingRight;
        }

        return isFacingRight;
        



    }

    protected void Die()
    {
        Destroy(gameObject);
    }

}
