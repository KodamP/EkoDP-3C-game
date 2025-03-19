using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Blockout");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
