using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public float maxPickupDistance = 2f;
    public GameObject raycastPoint;

    GameObject heldObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Do this when game mechanics are implemented
        /*if (Input.GetKeyDown(KeyCode.E))
        {
            // Removing the held object from the player
            if (heldObject != null)
                heldObject = null;

            RaycastHit hit;
            bool rayHit = Physics.Raycast(raycastPoint.transform.position, raycastPoint.transform.rotation.eulerAngles, out hit, maxPickupDistance);
            if (rayHit)
            {
                Debug.DrawRay(raycastPoint.transform.position, raycastPoint.transform.rotation.eulerAngles * maxPickupDistance, Color.yellow);
                // If we are doing a pickup
                if (hit.collider.CompareTag("Pickup") && heldObject == null)
                {
                    heldObject = hit.collider.gameObject;
                }

                // TODO: Buttons
            }
        }*/

    }
}
