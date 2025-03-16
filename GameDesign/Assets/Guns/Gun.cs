using System.Collections.Generic;
using System.Linq;
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

    public InputAction reloadAction;
    private int ammo = 5;
    private float reserve = Mathf.Infinity;
    private int magSize = 5;

    [SerializeField] private GameObject ammoUI;
    private List<GameObject> bulletList = new List<GameObject>();
    [SerializeField] private GameObject bulletPrefab;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();
        shootAction.Enable();
        shootAction.performed += Shoot;
        reloadAction.Enable();
        reloadAction.performed += Reload;

        //instantiate ammo UI
        for (int i = 0; i < ammo; i++)
        {
            AddUIBullet();
        }
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
        if (ammo != 0) 
        {
            player.Shoot(launchPower, direction.normalized);
            ammo -= 1;
            RemoveUIBullet();
        } else if (reserve > 0)
        {
            Reload(context);
        }
    }

    void Reload(InputAction.CallbackContext context) 
    {
        int neededBullets = magSize - ammo;
        reserve -= neededBullets;
        ammo = magSize;

        //update ui
        bulletList.Clear();
        for (int i = 0; i < ammo; i++)
        {
            AddUIBullet();
        }
    }

    void RemoveUIBullet()
    {
        GameObject tempBullet = bulletList.ElementAt(bulletList.Count - 1);
        GameObject.Destroy(tempBullet);
        bulletList.RemoveAt(bulletList.Count - 1);
    }

    void AddUIBullet() 
    {
        GameObject tempBullet = Instantiate(bulletPrefab, ammoUI.transform);
        bulletList.Add(tempBullet);
        tempBullet.transform.position += new Vector3(10*bulletList.Count,0);
    }
}
