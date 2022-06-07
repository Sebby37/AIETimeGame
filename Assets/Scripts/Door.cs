using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Parameters")]
    public DoorTypes doorType;

    [Header("Door Objects")]
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject topDoor;
    
    [Header("Trigger Parameters")]
    public List<Button> buttonTriggers = new List<Button>();

    private bool isOpen;
    private float animationTime = 0;
    private DoorState doorState;

    public enum DoorTypes
    {
        Sliding,
        TopDown
    }

    private enum DoorState
    {
        Closed,
        Opening,
        Open,
        Closing
    }
    
    // Function to create a small bump which will be used to smooth the door animations
    float SmoothOpening(float x)
    {
        // e^{-\left(2x-2\right)^{2}}+1
        return Mathf.Exp(-Mathf.Pow(17.5f * x - 2, 2))/5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Updating the animation timing variable
        animationTime += Time.deltaTime;
        
        // Checking if all buttons connected to the door are pressed
        bool allButtonsPressed = true;
        foreach (Button button in buttonTriggers)
            if (!button.IsPressed())
                allButtonsPressed = false;

        // Replace this with animations, as these can get V E R Y out of sync
        if (allButtonsPressed)
        {
            switch (doorState)
            {
                case DoorState.Closed:
                    OpenDoor();
                    break;
                case DoorState.Opening:
                    switch (doorType)
                    {
                        case DoorTypes.Sliding:
                            leftDoor.transform.Translate(new Vector3(0, 0, -SmoothOpening(animationTime)));
                            rightDoor.transform.Translate(new Vector3(0, 0, SmoothOpening(animationTime)));
                            break;
                    }
                    if (SmoothOpening(animationTime) < 0.001f && animationTime > 0.1f) doorState = DoorState.Open;
                    break;
            }
        }
        else
        {
            switch (doorState)
            {
                case DoorState.Open:
                    CloseDoor();
                    break;
                case DoorState.Closing:
                    switch (doorType)
                    {
                        case DoorTypes.Sliding:
                            leftDoor.transform.Translate(new Vector3(0, 0, SmoothOpening(animationTime)));
                            rightDoor.transform.Translate(new Vector3(0, 0, -SmoothOpening(animationTime)));
                            break;
                    }
                    if (SmoothOpening(animationTime) < 0.001f && animationTime > 0.1f) doorState = DoorState.Closed;
                    break;
            }
        }
    }

    void OpenDoor()
    {
        doorState = DoorState.Opening;
        animationTime = 0;
    }

    void CloseDoor()
    {
        doorState = DoorState.Closing;
        animationTime = 0;
    }
}
