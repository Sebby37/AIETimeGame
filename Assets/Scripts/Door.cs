using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Objects")]
    public Animator animator;
    public GameObject leftDoor;
    public GameObject rightDoor;
    
    [Header("Trigger Parameters")]
    public List<Button> buttonTriggers = new List<Button>();

    // Update is called once per frame
    void FixedUpdate()
    {
        // Checking if all buttons connected to the door are pressed
        bool allButtonsPressed = true;
        foreach (Button button in buttonTriggers)
            if (!button.IsPressed())
                allButtonsPressed = false;

        // Setting the "Open" parameter of the animator to be whether all buttons are pressed
        animator.SetBool("Open", allButtonsPressed);

        // Using a collider to make sure that players can't run through closing doors
        GetComponent<Collider>().enabled = !allButtonsPressed;
    }

    public void OpenDoor()
    {
        animator.SetBool("Open", true);

        GetComponent<Collider>().enabled = false;
    }

    public void CloseDoor()
    {
        animator.SetBool("Open", false);

        GetComponent<Collider>().enabled = true;
    }

    public bool IsOpen()
    {
        return animator.GetBool("Open");
    }
}
