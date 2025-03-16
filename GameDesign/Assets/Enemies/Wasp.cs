using UnityEngine;
using Pathfinding;

public class Wasp : Enemy
{
    public Transform target;
    public float nextWaypointDistance = 3f; // how close the wasp must be to the curr waypoint to move onto next one
    public float detectionRange = 10f; // distance b/w wasp and player for wasp to aggro

    private Path path;
    private int currentWaypoint = 0;
    private bool isEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private bool isFacingRight = false;
    private bool isChasing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("Move", 0f, 1f); // recalculate path every second
        
        
    }

    public override void Move()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer < detectionRange)
        {
            isChasing = true;
            ChasePlayer();

        }

        else
        {
            isChasing = false;
            Patrol();
        }
        

    }


    public override void ChasePlayer()
    {
        // if seeker done with updating last path
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }

    public override void Patrol()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0; // start at beginning of new path
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if  (currentWaypoint >= path.vectorPath.Count)
        {
            isEndOfPath = true;
            return;
        }
        else
        {
            isEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) 
        {
            currentWaypoint++;
        }
        // Flip sprite only if direction changes
        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
        {
            FlipSprite();
            isFacingRight = !isFacingRight;
        }

    }
}
