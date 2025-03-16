using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private GameObject playerObject;
    private Player player;

    public InputAction shootAction;
    Vector2 direction;
    private float launchPower = 10f;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();
        shootAction.Enable();
        shootAction.performed += Shoot;
    }

    void Update()
    {
        //stick to player
        transform.position = new Vector2(playerObject.transform.position.x + 0.21f, playerObject.transform.position.y - 0.22f);
        //rotate towards cursor
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
    }
    void Shoot(InputAction.CallbackContext context)
    {
        player.Shoot(launchPower,direction.normalized);
    }
}
