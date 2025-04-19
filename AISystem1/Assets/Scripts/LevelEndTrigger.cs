using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] GameObject levelEndUI;     // The LevelEndScreen UI Panel
    [SerializeField] Button restartButton;      // The Play Again button (assign in Inspector)
    [SerializeField] Button exitButton;         // The Exit button (assign in Inspector)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowEndScreen();
        }
    }

    void ShowEndScreen()
    {
        if (levelEndUI != null)
        {
            levelEndUI.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    // These functions are called from the buttons (via OnClick in Inspector)
    public void RestartLevel()   //Restart game
    {
        Time.timeScale = 1f; // Unpause time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()   //exits game
    {
        Time.timeScale = 1f;
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
