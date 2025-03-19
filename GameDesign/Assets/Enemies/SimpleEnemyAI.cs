using UnityEngine;

public class SimpleEnemyAI : EnemyAI
{
    [Header("Behaviour")]
    public float patrolDistance = 3f;

    private Rigidbody2D rb;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 currPoint;
    private bool isFacingRight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyShooting = GetComponent<EnemyShooting>();

        // Generate patrol points dynamically
        pointA = transform.position - new Vector3(patrolDistance, 0, 0);
        pointB = transform.position + new Vector3(patrolDistance, 0, 0);

        currPoint = pointA;
    }

    void Update()
    {
        Move();
    }

    public override void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        // Flip sprite only if direction changes
        isFacingRight = FlipSprite(direction.x, isFacingRight);
        
        // Constrain movement within patrol range
        float clampedX = Mathf.Clamp(transform.position.x, pointA.x, pointB.x);
        transform.position = new Vector2(clampedX, transform.position.y);
    }

    public override void Patrol()
    {
        Vector2 direction = (currPoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        if (Vector2.Distance(transform.position, currPoint) < 0.5f)
        {
            currPoint = (currPoint == pointB) ? pointA : pointB;
        }

        isFacingRight = FlipSprite(direction.x, isFacingRight);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointA, 0.5f);
        Gizmos.DrawWireSphere(pointB, 0.5f);
        Gizmos.DrawLine(pointA, pointB);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Visualize detection radius
    }
}
