using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering;
using System.Collections;


public class Mouse : Enemy
{
    [Header("Pathfinding")]
    public Transform target;
    public float detectionRange = 15f;
    public float pathUpdateSeconds = 1;
    public float nextWaypointDistance = 3f;

    [Header("Physics")]
    public float jumpNodeHeightRequirement = 0.8f; // height of next node to trigger jump
    public float jumpModifier = 0.8f; // strength/distance of jump
    public float jumpCheckOffset = 0.1f; // collider checking 
    public float jumpForce = 10f;

    [Header("Custom Behaviour")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private bool isGrounded = true;
    private Seeker seeker;
    private Rigidbody2D rb;
    private bool isFacingRight = false;
    private bool isOnCoolDown = false;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("Move", 0f, pathUpdateSeconds);
    }


    void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
    }

    public override void Move()
    {
        if (TargetInDistance())
        {
            ChasePlayer();

        }

        else
        {
            Patrol();
        }

        
    }

    public override void ChasePlayer()
    {
        // see if collision in path
        Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y, transform.position.z);
        int groundLayer = LayerMask.GetMask("Ground");

        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.5f, groundLayer);

        if (followEnabled && isGrounded && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        // jump
        if (jumpEnabled && isGrounded && !isOnCoolDown)
        {
            if (direction.y > jumpNodeHeightRequirement)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                StartCoroutine(JumpCoolDown());

            }
        }

        // movement 
        rb.AddForce(force);

        // next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // flip sprite only when direction changes
        if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
        {
            FlipSprite();
            isFacingRight = !isFacingRight;
        }

    }


    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }


    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.position) < detectionRange;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0; // start at beginning of new path
        }
    }
}