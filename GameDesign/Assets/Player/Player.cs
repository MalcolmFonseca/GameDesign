using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private float speed = 5f;
    private float jumpSpeed = 10f;
    private bool isJumping = false;
    Vector2 move;

    public InputAction moveAction;
    public InputAction jumpAction;

    void Start()
    {
        moveAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += Jump;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
            rigidbody2d.linearVelocity = new Vector2 (move.x*speed, rigidbody2d.linearVelocityY);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (!isJumping)
        {
            rigidbody2d.linearVelocity = new Vector2(rigidbody2d.linearVelocityX, jumpSpeed);
            isJumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }
}
