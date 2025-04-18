using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem;


public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    public Transform player;
    public int health = 3;
    public float speed = 2f;
    public float detectionRange = 5f;

    public GameObject droppedNotePrefab;
    public string[] lines;
    
    protected EnemyShooting enemyShooting;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


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


    public virtual void TakeDamage(int amount)
    {
        StartCoroutine(FlashRed());

        health -= amount;
        if (health <= 0)
        {
            if (droppedNotePrefab != null)
            {
                GameObject droppedNote = Instantiate(droppedNotePrefab);
                droppedNote.GetComponent<DroppedNote>().lines = lines;
                droppedNote.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
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

    // briefly change sprite colour upon taking damage
    private IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red; // Change color to red
            yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds
            sr.color = Color.white; // Reset color to default (white)
        }
    }

    private void OnDestroy()
    {
        if (droppedNotePrefab != null)
        {
            Instantiate(droppedNotePrefab);
        }
    }
}
