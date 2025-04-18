using UnityEngine;
using System.Collections;

public class CrowBossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public AudioSource audioSource;
    public AudioClip[] flapSounds;
    public AudioClip cawSound;
    public float flapInterval = 1f;
    private float flapTimer;

    [Header("Stats")]
    public int health = 5;

    [Header("Roam")]
    public float roamRadius = 5f;
    public float speed = 2f;

    [Header("Flight")]
    public float flightHeight = 3f;

    [Header("Dive Attack")]
    public float detectionRange = 8f;
    public float diveSpeed = 8f;
    public float windupTime = 0.5f;
    public float diveDuration = 1f;
    public float diveCooldown = 3f;
    public int contactDamage = 3;

    [Header("Bounds (world‑space)")]
    public Vector2 minBounds;   // bottom‑left corner of the cave
    public Vector2 maxBounds;   // top‑right corner of the cave

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 centerPos;
    private Vector2 roamTarget;
    private float lastDiveTime;
    private bool isDiving;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        centerPos = (Vector2)transform.position + Vector2.up * flightHeight;
        PickNewRoamPoint();
        lastDiveTime = -diveCooldown;

        // Prevent any unwanted rotation on collisions
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (isDiving) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (Time.time >= lastDiveTime + diveCooldown && dist < detectionRange)
        {
            StartCoroutine(DiveAtPlayer());
        }
        else
        {
            Roam();
        }

        ClampPosition();
    }

    private void ClampPosition()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minBounds.x, maxBounds.x);
        p.y = Mathf.Clamp(p.y, minBounds.y, maxBounds.y);
        transform.position = p;
    }

    void Roam()
    {
        Vector2 dir = (roamTarget - (Vector2)transform.position).normalized;
        rb.linearVelocity = dir * speed;
        sr.flipX = dir.x > 0f;

        if (Vector2.Distance(transform.position, roamTarget) < 0.5f)
            PickNewRoamPoint();

        flapTimer += Time.deltaTime;
        if (flapTimer >= flapInterval)
        {
            flapTimer -= flapInterval;
            if (flapSounds.Length > 0 && audioSource != null)
            {
                int i = Random.Range(0, flapSounds.Length);
                audioSource.PlayOneShot(flapSounds[i]);
            }
        }
    }

    IEnumerator DiveAtPlayer()
    {
        isDiving = true;
        lastDiveTime = Time.time;

        // wind‑up pause
        rb.linearVelocity = Vector2.zero;
        audioSource.PlayOneShot(cawSound);
        yield return new WaitForSeconds(windupTime);

        // dive phase
        float endTime = Time.time + diveDuration;
        while (Time.time < endTime)
        {
            Vector2 diveDir = (player.position - transform.position).normalized;
            rb.linearVelocity = diveDir * diveSpeed;
            sr.flipX = diveDir.x > 0f;
            yield return null;
        }

        // lift back up to hover altitude
        rb.linearVelocity = Vector2.zero;
        Vector3 pos = transform.position;
        pos.y = centerPos.y;
        transform.position = pos;

        isDiving = false;
        PickNewRoamPoint();
    }

    void PickNewRoamPoint()
    {
        roamTarget = centerPos + Random.insideUnitCircle * roamRadius;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null) player.TakeDamage(contactDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        StartCoroutine(FlashRed());
        health -= amount;
        if (health <= 0)
            Destroy(gameObject);
    }

    private IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
    }
}
