using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToStart : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) // if object is labeled player, loses life and moves back, or game over
        {
            player.transform.position = respawnPoint.position; // player moves back to designated position
        }
    }
}

