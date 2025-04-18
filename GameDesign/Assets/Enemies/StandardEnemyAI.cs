using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering;
using System.Collections;


public class StandardEnemyAI : EnemyAI
{
    [Header("Pathfinding")]
    public float pathUpdateSeconds = 1;
    public float nextWaypointDistance = 3f;
    public float oscillationAmplitude = 1f;

    [Header("Additional Behaviour")]
    public bool horizontalPatrol = true;
    public bool verticalPatrol = false;
    public bool jumpEnabled = true;
    public float jumpNodeHeightRequirement = 0.8f; // height of next node to trigger jump
    public float jumpModifier = 0.8f; // strength/distance of jump
    public float jumpCheckOffset = 0.1f; // collider checking 
    public float jumpForce = 10f;

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
        enemyShooting = GetComponent<EnemyShooting>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("Move", 0f, pathUpdateSeconds);
    }


    void FixedUpdate()
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
        Vector2 force = direction * speed * Time.deltaTime;

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
        isFacingRight = FlipSprite(direction.x, isFacingRight);

    }

    public override void Move()
    {
        base.Move();
        
    }

    public override void ChasePlayer()
    {
        // see if collision in path
        if (jumpEnabled)
        {
            Vector3 startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y, transform.position.z);
            int groundLayer = LayerMask.GetMask("Ground");

            isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.5f, groundLayer);
        }

        if (!seeker.IsDone()) return;


        if (!jumpEnabled || (jumpEnabled && isGrounded))
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }

    public override void Patrol()
    {
        float patrolMovement = Mathf.Sin(Time.time * speed) * oscillationAmplitude;
        
        if (horizontalPatrol)
        {
            rb.linearVelocity = new Vector2(patrolMovement, 0f);

            // Flip sprite when direction changes
            isFacingRight = FlipSprite(patrolMovement, isFacingRight);

        }

        if (verticalPatrol)
        {
            rb.linearVelocity = new Vector2(0f, patrolMovement);
        }



    }


    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
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