using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwitchScript : MonoBehaviour
{
    [SerializeField] DoorScript doorBehaviour; // make sure this matches your door script name
    [SerializeField] bool isDoorOpenSwitch = true;  // Set true to open the door
    [SerializeField] bool isDoorCloseSwitch = false; // Set true to close the door (optional)

    float switchSizeY;
    Vector3 switchUpPos;
    Vector3 switchDownPos;

    [SerializeField] float switchSpeed = 1f;  // Speed of switch animation
    [SerializeField] float switchDelay = 0.2f;  // Delay before switch pops back up

    bool isPressingSwitch = false;

    [Header("Events")]
    public UnityEvent onSwitchPressed = new UnityEvent();   // Called when switch is fully pressed down
    public UnityEvent onSwitchReleased = new UnityEvent();  // Called when switch is fully up
    public UnityEvent onSwitchActivated = new UnityEvent(); // Called when player steps on the switch

    void Awake()
    {
        switchSizeY = transform.localScale.y / 2;
        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }

    void Update()
    {
        if (isPressingSwitch)
        {
            MoveSwitchDown();
        }
        else
        {
            MoveSwitchUp();
        }
    }

    void MoveSwitchDown()
    {
        if (transform.position != switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchDownPos, switchSpeed * Time.deltaTime);
            if (transform.position == switchDownPos)
            {
                onSwitchPressed.Invoke();
            }
        }
    }

    void MoveSwitchUp()
    {
        if (transform.position != switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchUpPos, switchSpeed * Time.deltaTime);
            if (transform.position == switchUpPos)
            {
                onSwitchReleased.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player stepped on the switch!"); // Helps with debugging

            isPressingSwitch = !isPressingSwitch;

            onSwitchActivated.Invoke();

            if (isDoorOpenSwitch && !doorBehaviour.isDoorOpen)
            {
                doorBehaviour.SetDoorState(true); // Open the door
            }
            else if (isDoorCloseSwitch && doorBehaviour.isDoorOpen)
            {
                doorBehaviour.SetDoorState(false); // Close the door
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SwitchUpDelay(switchDelay));
        }
    }

    IEnumerator SwitchUpDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isPressingSwitch = false;
    }
}
