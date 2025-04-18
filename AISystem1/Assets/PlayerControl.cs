using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    private Vector3 input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = input * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);
    }
}
