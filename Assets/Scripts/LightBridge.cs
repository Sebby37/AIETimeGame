using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBridge : MonoBehaviour
{
    [Header("Bridge Parameters")]
    public float maximumLength = 50;
    public Material bridgeMaterial;

    [Header("Trigger Parameters")]
    public List<Button> buttonTriggers = new List<Button>();

    private GameObject bridge;

    void Start()
    {
        // Creating the bridge as a cube primitive
        bridge = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bridge.transform.parent = transform;
        bridge.transform.localPosition = Vector3.zero;
        bridge.transform.localScale = new Vector3(1.75f, 0.5f, 0.5f);
        bridge.tag = "Light Bridge";
        bridge.GetComponent<Renderer>().material = bridgeMaterial;
        bridge.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Checking if all buttons are pressed
        bool allButtonsPressed = true;
        foreach (Button button in buttonTriggers)
            if (!button.IsPressed())
                allButtonsPressed = false;

        if (allButtonsPressed)
        {
            RaycastHit hit;
            bool rayHit = Physics.Raycast(transform.position, transform.forward, out hit, maximumLength);

            // If the ray hit the player or itself, then it is treated as not hitting
            if (rayHit && (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Light Bridge")))
                rayHit = false;

            bridge.SetActive(true);
            
            // Scaling the bridge based on the hit distance / maximum length
            Vector3 tempScale = bridge.transform.localScale;
            tempScale.z = (rayHit ? hit.distance * 2 : maximumLength);
            bridge.transform.localScale = tempScale;

            // Setting the position of the bridge based on the hit distance / maximum length
            bridge.transform.localPosition = new Vector3(0, 0, (rayHit ? hit.distance / 1.25f : maximumLength / 2));
        }
        else
        {
            bridge.SetActive(false);
        }
    }
}
