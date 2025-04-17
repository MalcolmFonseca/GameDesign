using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedExitButton : MonoBehaviour
{
    public void BackToHomeScreen() 
    {
        SceneManager.LoadScene("HomeScreen");
    }
}
