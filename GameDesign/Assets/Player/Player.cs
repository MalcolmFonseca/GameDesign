using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections; 


public class Player : MonoBehaviour
{
    public int maxHealth = 20;
    public HealthBar healthBar;

    private int currentHealth;

    private Rigidbody2D rigidbody2d;
    private float speed = 1f;
    private Vector2 move;
    [SerializeField] private ContactFilter2D groundFilter;
    private bool isFacingRight = true;

    public InputAction moveAction;

    private Gun gun;

    void Start()
    {
        moveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        gun = GetComponentInChildren<Gun>();
    }

    void Update()
    {
        move = moveAction.ReadValue<Vector2>();

        if ((move.x > 0 && !isFacingRight) || (move.x < 0 && isFacingRight))
        {
            // rotate method for flipping so bullets change direction accordingly
            transform.Rotate(0f, 180f, 0f);

            isFacingRight = !isFacingRight;
        }
    }

    private void FixedUpdate()
    {
        if (Grounded())
        {
            float xSpeed = rigidbody2d.linearVelocityX + move.x * speed;
            xSpeed = Mathf.Clamp(xSpeed, -5, 5);
            rigidbody2d.linearVelocity = new Vector2(xSpeed, rigidbody2d.linearVelocityY);
        } else
        {
            //IN AIR
            float xSpeed = rigidbody2d.linearVelocityX + move.x * (speed/10);
            xSpeed = Mathf.Clamp(xSpeed, -5, 5);
            rigidbody2d.linearVelocity = new Vector2(xSpeed, rigidbody2d.linearVelocityY);
        }
    }

    // TODO: Game over screen upon death
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        StartCoroutine(FlashColor(Color.red)); // flash sprite red


        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


    public void Shoot(float launchPower, Vector2 direction)
    {
        rigidbody2d.linearVelocity = new Vector2(-direction.x * launchPower, -direction.y * launchPower);
    }

    //check if on ground
    private bool Grounded()
    {
        return rigidbody2d.IsTouching(groundFilter);
    }

    // briefly change sprite colour (ex. healing, damaage)
    private IEnumerator FlashColor(Color color)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color; // Change color
            yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds
            sr.color = Color.white; // Reset color to default (white)
        }
    }


    // on collision with items
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: each health potion hardcoded to do 5 points of healing, consider dynamic assignment
        if (collision.CompareTag("HealthPot"))
        {

            Destroy(collision.gameObject);
            currentHealth = currentHealth + 5 <= maxHealth ? currentHealth + 5 : maxHealth;
            healthBar.SetHealth(currentHealth);

            StartCoroutine(FlashColor(Color.green)); // flash sprite green

        }

        if (collision.CompareTag("Ammo"))
        {

            Destroy(collision.gameObject);

            if (gun != null) gun.SpecialReload();

        }
    }

}
