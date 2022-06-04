using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiIndicators : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite redLed;
    public Sprite greenLed;

    [Header("Indicator GameObjects")]
    public GameObject reverseTimeGameobject;
    public GameObject stopTimeGameobject;
    public GameObject slowTimeGameobject;

    // The image components that will have their source image changed
    private Image reverseTimeImage;
    private Image stopTimeImage;
    private Image slowTimeImage;

    // Start is called before the first frame update
    void Start()
    {
        // Getting the image components of the UI Image Gameobjects
        reverseTimeImage = reverseTimeGameobject.GetComponent<Image>();
        stopTimeImage = stopTimeGameobject.GetComponent<Image>();
        slowTimeImage = slowTimeGameobject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Replace the simple keycode check with checking a boolean to make sure the player can actually perform these actions
        reverseTimeImage.sprite = ReverseTimeComponent.rewinding ? greenLed : redLed;
        stopTimeImage.sprite = Input.GetKey(KeyCode.Mouse1) ? greenLed : redLed;
        slowTimeImage.sprite = Input.GetKey(KeyCode.Q) ? greenLed : redLed;
    }
}
