using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    public AudioSource audioSource;
    public AudioClip[] standardShoot;
    public AudioClip[] specialShoot;

    [SerializeField] private GameObject ammoUI;
    private List<GameObject> bulletList = new List<GameObject>();
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject reserveTextObject;

    public GameObject standardBullet;

    public int bulletDamage = 1;


    [Header("Collectible Ammo")]
    public InputAction specialShootAction;
    private float specialLaunchPower = 20f;
    private int specialAmmo = 0;

    [SerializeField] private GameObject specialAmmoUI; // UI Parent for Special Ammo
    private List<GameObject> specialBulletList = new List<GameObject>();

    [SerializeField] private GameObject specialBulletPrefab; // UI Icon for Special Ammo

    public GameObject specialBullet;

    private List<GunUpgrade> upgrades = new List<GunUpgrade>();

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<Player>();

        standardShootAction.Enable();
        standardShootAction.performed += ctx => Shoot(standardBullet, ref ammo, standardLaunchPower, true);

        specialShootAction.Enable();
        specialShootAction.performed += ctx => Shoot(specialBullet, ref specialAmmo, standardLaunchPower + 10f, false);

        reloadAction.Enable();
        reloadAction.performed += StandardReload;

        

        //instantiate standard ammo UI
        for (int i = 0; i < ammo; i++)
        {
            AddUIBullet(bulletPrefab, ammoUI.transform, bulletList);
        }

        UpdateText();
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

        //check if paused
        if (player.paused == true)
        {
            standardShootAction.Disable();
            reloadAction.Disable();
            specialShootAction.Disable();
        } else if (player.paused == false)
        {
            standardShootAction.Enable();
            reloadAction.Enable();
            specialShootAction.Enable();
        }
    }

    // Merged Shoot function for standard and special bullets
    void Shoot(GameObject bulletPrefab, ref int ammoCount, float launchPower, bool canReload)
    {
            if (ammoCount > 0)
            {
                Instantiate(bulletPrefab, bulletPos.position, bulletPos.rotation);
                player.Shoot(launchPower, transform.right);
                ammoCount--;

                // pick and play a random shooting sound
                AudioClip[] pool = canReload ? standardShoot : specialShoot;
                if (pool != null && pool.Length > 0)
                {
                    int idx = Random.Range(0, pool.Length);
                    audioSource.PlayOneShot(pool[idx]);
                }


            // remove ammo from correct list
            if (canReload) RemoveUIBullet(bulletList);

                else RemoveUIBullet(specialBulletList);
            }
            else if (canReload && reserve > 0)
            {
                StandardReload(new InputAction.CallbackContext());
            }
    }


    void StandardReload(InputAction.CallbackContext context) 
    {
        if (player.Grounded())
        {
            int neededBullets = magSize - ammo;
            reserve -= neededBullets;
            ammo = magSize;

            //update ui
            bulletList.Clear();
            for (int i = 0; i < ammo; i++)
            {
                AddUIBullet(bulletPrefab, ammoUI.transform, bulletList);
            }

            UpdateText();
        }
    }

    public void SpecialReload()
    {
        specialAmmo++;
        Debug.Log("SpecAmmo: " + specialAmmo);
        AddUIBullet(specialBulletPrefab, specialAmmoUI.transform, specialBulletList);
    }

    void AddUIBullet(GameObject bulletPrefab, Transform ammoUI, List<GameObject> bulletList)
    {
        GameObject tempBullet = Instantiate(bulletPrefab, ammoUI);
        bulletList.Add(tempBullet);
        tempBullet.transform.position += new Vector3(15 * bulletList.Count, -30);
    }

    void RemoveUIBullet(List<GameObject> bulletList)
    {
        if (bulletList.Count > 0)
        {
            GameObject tempBullet = bulletList[bulletList.Count - 1];
            Destroy(tempBullet);
            bulletList.RemoveAt(bulletList.Count - 1);
        }
    }

    public void attachUpgrade(GunUpgrade upgrade)
    {
        upgrades.Add(upgrade);
        //add all modifiers, will need to be added to when new upgrades are added in future versions
        magSize += upgrade.magSizeMod;
        standardLaunchPower += upgrade.launchPowerMod;
        bulletDamage += upgrade.damageMod;
    }

    public void removeUpgrade(GunUpgrade upgrade)
    {
        upgrades.Remove(upgrade);
        //remove all modifiers, will need to be added to when new upgrades are added in future versions
        magSize -= upgrade.magSizeMod;
        standardLaunchPower -= upgrade.launchPowerMod;
        bulletDamage -= upgrade.damageMod;
    }

    private void UpdateText()
    {
        TMP_Text reserveText = reserveTextObject.GetComponent<TMP_Text>();
        if (reserve == Mathf.Infinity)
        {
            reserveText.text = "∞";
        }
        else
        {
            reserveText.text = reserve.ToString();
        }
        GameObject lastBullet = bulletList.Last();
        reserveTextObject.transform.position = new Vector3(lastBullet.transform.position.x + 20, lastBullet.transform.position.y - 15);
    }
}
