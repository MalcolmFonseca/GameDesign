using UnityEngine;

public class PowerUp1 : GunUpgrade
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Gun gun = other.GetComponentInChildren<Gun>();
            if (gun != null)
            {
                gun.attachUpgrade(this);
            }

            Destroy(gameObject);
        }
    }
}

