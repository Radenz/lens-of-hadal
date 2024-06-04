using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private Image _scrim;

    public void Pause()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        _scrim.raycastTarget = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        _scrim.raycastTarget = false;
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
