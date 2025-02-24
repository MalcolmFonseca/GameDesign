using UnityEngine;

public class TumbleweedController : MonoBehaviour {
    public float horizontalSpeed = 5f;  // Speed moving left
    public float bounceForce = 5f;      // Upward force for bounce
    public int maxBounces = 5;          

    public AudioClip triggerSound;
    public AudioClip bounceSound;

    private int bounceCount = 0;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        if (triggerSound != null) {
            audioSource.PlayOneShot(triggerSound);
        }

        rb.linearVelocity = new Vector2(-horizontalSpeed, bounceForce);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Bounce off the ground on impact
        if (bounceCount < maxBounces && collision.gameObject.CompareTag("Ground")) {
            rb.linearVelocity = new Vector2(-horizontalSpeed, bounceForce);
            bounceCount++;

            if (bounceSound != null) {
                audioSource.PlayOneShot(bounceSound);
            }
        }
    }
}
