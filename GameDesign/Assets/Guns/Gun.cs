using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private GameObject playerObject;
    private Player player;

    public Transform bulletPos;

    [Header("Standard Ammo")]
    public InputAction standardShootAction;
    Vector2 direction;
    private float standardLaunchPower = 10f;
    
    public InputAction reloadAction;
    private int ammo = 5;
    private float reserve = Mathf.Infinity;
    private int magSize = 5;

    [SerializeField] private GameObject ammoUI;
    private List<GameObject> bulletList = new List<GameObject>();
    [SerializeField] private GameObject bulletPrefab;

    public GameObject standardBullet;


    [Header("Collectible Ammo")]
    public InputAction specialShootAction;
    private float specialLaunchPower = 10f;
    private int specialAmmo = 0;

    public GameObject specialBullet;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();

        standardShootAction.Enable();
        standardShootAction.performed += ctx => Shoot(standardBullet, ref ammo, standardLaunchPower, true);

        specialShootAction.Enable();
        specialShootAction.performed += ctx => Shoot(specialBullet, ref specialAmmo, specialLaunchPower, false);

        reloadAction.Enable();
        reloadAction.performed += StandardReload;

        

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

    // Merged Shoot function for standard and special bullets
    void Shoot(GameObject bulletPrefab, ref int ammoCount, float launchPower, bool canReload)
    {
        if (ammoCount > 0)
        {
            Instantiate(bulletPrefab, bulletPos.position, bulletPos.rotation);
            player.Shoot(launchPower, transform.right);
            ammoCount--;

            if (canReload)
                RemoveUIBullet();
        }
        else if (canReload && reserve > 0)
        {
            StandardReload(new InputAction.CallbackContext());
        }
    }


    void StandardReload(InputAction.CallbackContext context) 
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

    public void SpecialReload()
    {
        specialAmmo++;
        Debug.Log("SpecAmmo: " + specialAmmo);
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
