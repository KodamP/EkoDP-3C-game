using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("IslandLevel");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
