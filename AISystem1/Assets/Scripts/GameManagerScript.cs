using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] public GameObject gameOverUI;   //Game over UI panel shows when player dies

    public GameObject player;   //references player

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;   //hide cursor during gameplay
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverUI.activeInHierarchy)   //show cursor when game over screen active
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else   //hide cursor during normal gameplay
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void gameOver()  //goes to game over screen when health at 0
    {
        gameOverUI.SetActive(true);
    }
    public void start()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // loads scene according ot build index
    }
    public void quit()   //exits application
    {
        Application.Quit();
    }

}
