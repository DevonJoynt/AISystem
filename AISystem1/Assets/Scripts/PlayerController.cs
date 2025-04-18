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
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        CheckBounds();
        CheckForAttack();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void ProcessInput()
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");

        movement = new Vector3(dx, 0, dz).normalized * speed;

        if (movement.sqrMagnitude > 0)
        {
            // Rotate smoothly toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void MovePlayer()
    {
        Vector3 newPosition = rb.position + movement * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void CheckBounds()
    {
        float x = transform.position.x;
        float z = transform.position.z;
        x = Mathf.Clamp(x, MIN_X, MAX_X);
        z = Mathf.Clamp(z, MIN_Z, MAX_Z);

        transform.position = new Vector3(x, transform.position.y, z);
    }

    private void CheckForAttack()
    {
        bool checkAttack = Input.GetButtonDown("Fire1");

        if (checkAttack && attackGO.activeSelf == false)
        {
            attackGO.SetActive(true);
        }
    }

}
