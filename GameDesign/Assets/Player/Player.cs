using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private float speed = 1f;
    Vector2 move;

    public InputAction moveAction;

    void Start()
    {
        moveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        float xSpeed = rigidbody2d.linearVelocityX + move.x * speed;
        xSpeed = Mathf.Clamp(xSpeed, -5, 5);
        rigidbody2d.linearVelocity = new Vector2(xSpeed, rigidbody2d.linearVelocityY);
    }

    public void Shoot(float launchPower, Vector2 direction)
    {
        rigidbody2d.linearVelocity = new Vector2(-direction.x * launchPower, -direction.y * launchPower);
    }
}
