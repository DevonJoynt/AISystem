using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public GameManagerScript gameManager;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            gameManager.gameOver();
        }
    }
}
