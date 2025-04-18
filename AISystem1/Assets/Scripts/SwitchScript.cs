using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchScript : MonoBehaviour
{
    [SerializeField] DoorScript doorScript;  //door is controlled by this switch
    [SerializeField] bool isDoorOpenSwitch;   //determines if this switch opens door
    [SerializeField] bool isDoorCloseSwitch;   //determines if this switch closes door

    float switchSizeY;
    Vector3 switchUpPos;   //unpressed position
    Vector3 switchDownPos;   //pressed position

    [SerializeField] float switchSpeed = 1f;   //speed switch moves when pressed
    [SerializeField] float switchDelay = 0.2f;   //delay before switch released

    bool isPressingSwitch = false;   //is switch currently being pressed

    // Removed: requiredItem & inventory check

    // Add Unity Events
    [Header("Events")]
    public UnityEvent onSwitchPressed = new UnityEvent();  //event fires when switch is pressed
    public UnityEvent onSwitchReleased = new UnityEvent();   //event fires when switch is released
    public UnityEvent onSwitchActivated = new UnityEvent();   //event fires when switch is activated

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPressingSwitch = !isPressingSwitch;

            // Invoke success event
            onSwitchActivated.Invoke();

            if (isDoorOpenSwitch && !doorScript.isDoorOpen)
            {
                doorScript.SetDoorState(true);
            }
            else if (isDoorCloseSwitch && doorScript.isDoorOpen)
            {
                doorScript.SetDoorState(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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
