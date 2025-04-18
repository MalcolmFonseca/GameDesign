using UnityEngine;

public class UpgradeText : MonoBehaviour
{
    float timer;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            Destroy(gameObject);
        }
    }
}
