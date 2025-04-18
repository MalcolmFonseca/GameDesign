using UnityEngine;
using TMPro;

public class PowerUp1 : GunUpgrade
{
    public GameObject upgradeTextPrefab;
    private GameObject UI;

    private void Start()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Gun gun = other.GetComponentInChildren<Gun>();
            if (gun != null)
            {
                gun.attachUpgrade(this);
            }

            if (magSizeMod > 0)
            {
                GameObject upgradeText = Instantiate(upgradeTextPrefab, UI.transform);
                upgradeText.GetComponent<TMP_Text>().text = "+" + magSizeMod + " Magazine Size";
            }

            if (launchPowerMod > 0)
            {
                GameObject upgradeText = Instantiate(upgradeTextPrefab, UI.transform);
                upgradeText.GetComponent<TMP_Text>().text = "+" + launchPowerMod + " Launch Power";
                upgradeText.transform.position = new Vector3(upgradeText.transform.position.x, upgradeText.transform.position.y + 30);
            }

            if (damageMod > 0)
            {
                GameObject upgradeText = Instantiate(upgradeTextPrefab, UI.transform);
                upgradeText.GetComponent<TMP_Text>().text = "+" + damageMod + " Damage";
                upgradeText.transform.position = new Vector3(upgradeText.transform.position.x, upgradeText.transform.position.y + 60);
            }

            Destroy(gameObject);
        }
    }
}

