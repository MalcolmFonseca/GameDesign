using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D collider2d;
    private float speed = 1f;
    private Vector2 move;
    [SerializeField] private ContactFilter2D groundFilter;

    public InputAction moveAction;

    void Start()
    {
        moveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
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

    public void Shoot(float launchPower, Vector2 direction)
    {
        rigidbody2d.linearVelocity = new Vector2(-direction.x * launchPower, -direction.y * launchPower);
    }

    //check if on ground
    private bool Grounded()
    {
        return rigidbody2d.IsTouching(groundFilter);
}
}
