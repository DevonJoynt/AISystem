using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DoorStateEvent : UnityEvent<bool> { }

public class DoorScript : MonoBehaviour
{
    public bool isDoorOpen = false;   //tracks if door is currently open
    Vector3 doorClosedPos;   //position when door is closed
    Vector3 doorOpenPos;  //position when door is open
    [SerializeField] float doorSpeed = 10f;  //speed of door opening

    // Add Unity Events
    [Header("Events")]
    public DoorStateEvent onDoorStateChanged = new DoorStateEvent();  //event fires when door changes state
    public UnityEvent onDoorOpened = new UnityEvent();  //event fires when door finishes opening
    public UnityEvent onDoorClosed = new UnityEvent();  //event fires when door finishes opening

    void Awake()
    {
        doorClosedPos = transform.position;  //stores inital closed position
        doorOpenPos = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);   //calculates the open position
    }

    void Update()
    {
        if (isDoorOpen)
        {
            OpenDoor();
        }
        else if (!isDoorOpen)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (transform.position != doorOpenPos)   //moves only if not at target position
        {
            transform.position = Vector3.MoveTowards(transform.position, doorOpenPos, doorSpeed * Time.deltaTime);

            // open event triggered when door reaches open position
            if (transform.position == doorOpenPos)
            {
                onDoorOpened.Invoke();
            }
        }
    }

    void CloseDoor()
    {
        if (transform.position != doorClosedPos)   //moves only if not at target position
        {
            transform.position = Vector3.MoveTowards(transform.position, doorClosedPos, doorSpeed * Time.deltaTime);

            // closed event triggered when door reaches closed position
            if (transform.position == doorClosedPos)
            {
                onDoorClosed.Invoke();
            }
        }
    }

    // Method to set door state with event
    public void SetDoorState(bool open)
    {
        if (isDoorOpen != open)
        {
            isDoorOpen = open;
            onDoorStateChanged.Invoke(open);   //notifies listeners about door state change
        }
    }
}