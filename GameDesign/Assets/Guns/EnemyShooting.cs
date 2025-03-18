using UnityEngine;
using System.Collections;


public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public float shootCooldown; // colldown between each bullet firing
    public int numShots = 1;

    private float timer;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > shootCooldown)
        {
            timer = 0;
            StartCoroutine(ShootBurst());
        }
    }

    IEnumerator ShootBurst()
    {
        for (int i = 0; i < numShots; i++)
        {
            Shoot();
            yield return new WaitForSeconds(0.2f); 
        }
    }


    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
