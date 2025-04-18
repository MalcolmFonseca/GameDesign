using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public InputAction jumpAction;
    public InputAction pauseAction;

    public bool paused = false;
    private GameObject pausedUIObject;
    private TMP_Text pausedText;
    private Button exitButton;

    private Gun gun;

    public Vector3 respawnPoint;

    //make singleton
    void Awake()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        if (player.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Respawn();
        moveAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += Jump;
        pauseAction.Enable();
        pauseAction.performed += Pause;
        rigidbody2d = GetComponent<Rigidbody2D>();

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        gun = GetComponentInChildren<Gun>();

        pausedUIObject = GameObject.FindGameObjectWithTag("PausedUI");
        pausedText = pausedUIObject.GetComponentInChildren<TMP_Text>();
        exitButton = pausedUIObject.GetComponentInChildren<Button>();
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
            Respawn();
        }
    }


    public void Shoot(float launchPower, Vector2 direction)
    {
        if (!Grounded())
        {
            rigidbody2d.linearVelocity = new Vector2(-direction.x * launchPower, -direction.y * launchPower);
        }
    }

    //check if on ground
    public bool Grounded()
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

    public void Respawn()
    {
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        pausedUIObject = GameObject.FindGameObjectWithTag("PausedUI");
        pausedText = pausedUIObject.GetComponentInChildren<TMP_Text>();
        exitButton = pausedUIObject.GetComponentInChildren<Button>();

        currentHealth = maxHealth;
        healthBar.SetHealth(maxHealth);
        
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Prototyping")
        {
        respawnPoint = new Vector3(-83.4000015f,-12.2700005f,0);
        }
        else if (sceneName == "Underground")
        {
        respawnPoint = new Vector3(131.009995f,11.1199999f,0); 
        }
        else if (sceneName == "BossFight")
        {
        respawnPoint = new Vector3(75.8000031f,-28.7000008f,0);
        }
        else
        {
        respawnPoint = new Vector2(0f, 0f); //default
        }

        
        transform.position = respawnPoint;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (Grounded())
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocityX, 10);
        }
    }

    //Attaching pause to character for ease
    private void Pause(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            pausedText.SetText("Paused");
            exitButton.transform.localScale = new Vector3(1, 1, 1);
            Time.timeScale = 0;
            paused = true;
            moveAction.Disable();
            jumpAction.Disable();
        } else
        {
            pausedText.SetText("");
            exitButton.transform.localScale = new Vector3(0, 0, 0);
            Time.timeScale = 1;
            paused = false;
            moveAction.Enable();
            jumpAction.Enable();
        }
    }
}
