using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public const float MIN_X = -50;
    public const float MAX_X = 300;
    public const float MIN_Z = -50;
    public const float MAX_Z = 50;

    [SerializeField]
    private float speed = 20;
    [SerializeField]
    private float rotateSpeed = 45;

    [SerializeField]
    private GameObject attackGO;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        ProcessInput();
        CheckForAttack();
    }

    void FixedUpdate()
    {
        MovePlayerWithVelocity();
        ClampPlayerWithinBounds();
    }

    private void ProcessInput()
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");

        movement = new Vector3(dx, 0, dz).normalized * speed;

        if (movement.sqrMagnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void MovePlayerWithVelocity()
    {
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }

    private void ClampPlayerWithinBounds()
    {
        Vector3 clampedPosition = rb.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, MIN_X, MAX_X);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, MIN_Z, MAX_Z);

        // Only apply clamping if player is out of bounds (prevents jitter)
        if (clampedPosition != rb.position)
        {
            rb.position = clampedPosition;
        }
    }

    private void CheckForAttack()
    {
        bool checkAttack = Input.GetButtonDown("Fire1");

        if (checkAttack && attackGO.activeSelf == false)
        {
            attackGO.SetActive(true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
    }
}
