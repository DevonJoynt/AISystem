using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public GameManagerScript gameManager;
    private void OnTriggerEnter(Collider collision)  // if player enters trigger - game over screen appears
    {
        if (collision.tag == "Player")
        {
            gameManager.gameOver();
        }
    }
}
