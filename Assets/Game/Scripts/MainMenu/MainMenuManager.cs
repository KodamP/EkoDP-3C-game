using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Gameplay1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
