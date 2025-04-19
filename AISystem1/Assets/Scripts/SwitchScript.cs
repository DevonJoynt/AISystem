using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwitchScript : MonoBehaviour
{
    [SerializeField] DoorScript doorBehaviour; // make sure this matches your door script name
    [SerializeField] bool isDoorOpenSwitch = true;  // Set true to open the door
    [SerializeField] bool isDoorCloseSwitch = false; // Set true to close the door (optional)

    float switchSizeY;         // Half-height of switch for movement calculation
    Vector3 switchUpPos;       
    Vector3 switchDownPos;     

    [SerializeField] float switchSpeed = 1f;  // Speed of switch animation
    [SerializeField] float switchDelay = 0.2f;  // Delay before switch pops back up

    bool isPressingSwitch = false; // Tracks switch press state

    [Header("Events")]  //activated when switch is pressed or released
    public UnityEvent onSwitchPressed = new UnityEvent();   
    public UnityEvent onSwitchReleased = new UnityEvent();  
    public UnityEvent onSwitchActivated = new UnityEvent(); 

    void Awake()
    {
        // Calculate half-height and set target positions for animation
        switchSizeY = transform.localScale.y / 2;
        switchUpPos = transform.position;
        switchDownPos = new Vector3(transform.position.x, transform.position.y - switchSizeY, transform.position.z);
    }

    void Update()
    {
        // Choose animation based on switch state
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
        // Animate movement toward pressed position
        if (transform.position != switchDownPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchDownPos, switchSpeed * Time.deltaTime);

            // Fire event when pressed position reached
            if (transform.position == switchDownPos)
            {
                onSwitchPressed.Invoke();
            }
        }
    }

    void MoveSwitchUp()
    {
        // Animate movement toward unpressed position
        if (transform.position != switchUpPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, switchUpPos, switchSpeed * Time.deltaTime);

            // Fire event when switch reaches top
            if (transform.position == switchUpPos)
            {
                onSwitchReleased.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Triggered when player touches switch
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player stepped on the switch!"); // Helps with debugging

            isPressingSwitch = !isPressingSwitch; // Toggle switch state

            onSwitchActivated.Invoke(); // Fire activation event

            // Handle door open logic
            if (isDoorOpenSwitch && !doorBehaviour.isDoorOpen)
            {
                doorBehaviour.SetDoorState(true); // Open the door
            }
            // Handle door close logic
            else if (isDoorCloseSwitch && doorBehaviour.isDoorOpen)
            {
                doorBehaviour.SetDoorState(false); // Close the door
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        // Triggered when player steps off switch
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SwitchUpDelay(switchDelay)); // Wait before lifting switch
        }
    }

    IEnumerator SwitchUpDelay(float waitTime)
    {
        // Wait for delay duration
        yield return new WaitForSeconds(waitTime);

        // Lift switch back up
        isPressingSwitch = false;
    }
}

