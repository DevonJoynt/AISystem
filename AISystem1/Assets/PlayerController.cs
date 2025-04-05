using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public const float MIN_X = -50;
    public const float MAX_X = 50;
    public const float MIN_Z = -50;
    public const float MAX_Z = 50;

    [SerializeField]
    private float speed = 20;
    [SerializeField]
    private float rotateSpeed = 45;

    [SerializeField]
    private GameObject attackGO;

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CheckBounds();

        CheckForAttack();
    }

    private void MovePlayer()
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(dx, 0, dz);
        dir.Normalize();

        if (dir.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        }

        transform.Translate(dir * speed * Time.deltaTime, Space.World);
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
