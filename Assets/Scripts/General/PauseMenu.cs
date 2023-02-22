using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    [SerializeField]
    private GameObject pauseMenu;
    public static bool isPaused;

    [HideInInspector] public static PauseMenu instance;
    private void Awake() {instance = this;}

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void openPauseMenu()
    {
        if (pauseMenu.activeInHierarchy)
        {
            ResumeGame();
        } else 
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void BackMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
