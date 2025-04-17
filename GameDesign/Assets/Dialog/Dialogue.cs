using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    Player player;
    Gun gun;

    void Start()
    {
        textComponent.text = string.Empty;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.paused = true;
        player.moveAction.Disable();
        player.jumpAction.Disable();
        gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
        gun.standardShootAction.Disable();
        gun.specialShootAction.Disable();
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            if (textComponent.text == lines[index])
            {
                NextLine();
            } else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else
        {
            Destroy(gameObject);
            player.moveAction.Enable();
            player.jumpAction.Enable();
            gun.standardShootAction.Enable();
            gun.specialShootAction.Enable();
            player.paused = false;
        }
    }
}
