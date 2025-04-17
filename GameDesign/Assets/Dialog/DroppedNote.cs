using UnityEngine;

public class DroppedNote : MonoBehaviour
{
    public string[] lines;

    public GameObject dialoguePrefab;

    private GameObject UI;

    private void Start()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject dialogue = Instantiate(dialoguePrefab, UI.transform);
        dialogue.GetComponent<Dialogue>().lines = lines;
        Destroy(gameObject);
    }
}
