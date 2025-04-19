using UnityEngine;
using System.Collections;

// Requires Rigidbody component on this GameObject
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // World boundaries for player movement
    public const float MIN_X = -50;
    public const float MAX_X = 300;
    public const float MIN_Z = -50;
    public const float MAX_Z = 50;

    [SerializeField]
    private float speed = 20;             // Movement speed
    [SerializeField]
    private float rotateSpeed = 45;       // Rotation speed

    [SerializeField]
    private GameObject attackGO;          // GameObject used for attack effect

    private Rigidbody rb;                 // Reference to Rigidbody component
    private Vector3 movement;             // Stores input-based movement direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();                       // Get Rigidbody reference
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth Rigidbody movement
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevent high-speed collision issues
    }

    void Update()
    {
        ProcessInput();      // Read player input for movement
        CheckForAttack();    // Detect attack input
    }

    void FixedUpdate()
    {
        MovePlayerWithVelocity();   // Apply movement to Rigidbody
        ClampPlayerWithinBounds();  // Restrict position to map limits
    }

    private void ProcessInput()
    {
        float dx = Input.GetAxis("Horizontal");  // Get horizontal input
        float dz = Input.GetAxis("Vertical");    // Get vertical input

        movement = new Vector3(dx, 0, dz).normalized * speed; // Create movement vector

        if (movement.sqrMagnitude > 0)
        {
            // Rotate player toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void MovePlayerWithVelocity()
    {
        // Apply movement to Rigidbody while preserving vertical velocity
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    private void ClampPlayerWithinBounds()
    {
        Vector3 clampedPosition = rb.position;

        // Clamp X and Z positions within defined limits
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, MIN_X, MAX_X);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, MIN_Z, MAX_Z);

        // Apply position clamp only when needed
        if (clampedPosition != rb.position)
        {
            rb.position = clampedPosition;
        }
    }

    private void CheckForAttack()
    {
        bool checkAttack = Input.GetButtonDown("Fire1"); // Detect attack input

        // Activate attack object if not already active
        if (checkAttack && attackGO.activeSelf == false)
        {
            attackGO.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Log name of collided object
        Debug.Log("Collided with: " + collision.gameObject.name);
    }
}
