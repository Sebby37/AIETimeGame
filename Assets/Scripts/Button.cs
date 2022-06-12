using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public ButtonTypes buttonType;
    public bool invertPress = false;

    private bool pressed;

    public enum ButtonTypes
    {
        Pressable,
        Weighted,
        CubeOnly
    }

    void Start()
    {
        // TODO: Actually implement inverted buttons
        pressed = invertPress;
    }

    public bool IsPressed() 
    {
        return pressed;
    }

    public void Press()
    {
        if (pressed) return;

        pressed = !invertPress;
        Debug.Log($"Button pressed! ({gameObject.name})");
    }

    public void UnPress()
    {
        if (!pressed) return;

        pressed = invertPress;
        Debug.Log($"Button un-pressed! ({gameObject.name})");
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandlePressCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandlePressCollision(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        HandlePressCollision(collision, false);
    }

    // Function to handle collisions and pressing on a weighted/cube button 
    private void HandlePressCollision(Collision collision, bool press = true)
    {
        if (buttonType == ButtonTypes.Pressable) return;

        if (collision.contactCount > 0 && !collision.GetContact(0).thisCollider.gameObject.CompareTag("Weighted Button")) return;

        switch (buttonType)
        {
            case ButtonTypes.Weighted:
                if (press)
                    Press();
                else
                    UnPress();
                break;
            case ButtonTypes.CubeOnly:
                if (collision.gameObject.CompareTag("Pickup"))
                    if (press)
                        Press();
                    else
                        UnPress();
                break;
        }
    }
}
