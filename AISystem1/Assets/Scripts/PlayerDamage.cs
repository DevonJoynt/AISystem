using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private Vector3 startPosition;
    private Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Make sure the enemy GameObject is tagged "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ResetToStart();
        }
    }

    private void ResetToStart()
    {
        Debug.Log("Player hit the enemy. Resetting to start position.");

        // Stop movement
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Move back to original start position
        transform.position = startPosition;
    }
}
