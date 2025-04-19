using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToStart : MonoBehaviour
{
    public GameObject player;             // Reference to player GameObject
    public Transform respawnPoint;        // Position player will reset to
    public AudioClip hitSound;            // Assign hit sound in Inspector

    private AudioSource audioSource;      // Audio source to play sound

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  // Get AudioSource on same GameObject
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Play hit sound if assigned
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // Move player to respawn position
            player.transform.position = respawnPoint.position;
        }
    }
}


