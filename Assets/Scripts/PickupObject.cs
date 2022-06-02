using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public float maxPickupDistance = 3f;
    public GameObject raycastPoint;
    public GameObject pickupParent;

    private GameObject heldObject;

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 direction = target.position - origin.position
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Do this when game mechanics are implemented
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Removing the held object from the player
            RaycastHit hit;
            bool rayHit = Physics.Raycast(raycastPoint.transform.position, raycastPoint.transform.forward, out hit, maxPickupDistance);
            if (rayHit)
            {
                Debug.DrawRay(raycastPoint.transform.position, raycastPoint.transform.forward * maxPickupDistance, Color.yellow);
                // If we are doing a pickup
                if (!IsObjectHeld())
                {
                    if (hit.collider.CompareTag("Pickup"))
                    {
                        heldObject = hit.collider.gameObject;
                    }
                }
                else
                {
                    DropHeldObject();
                }
            }
        }
        if (IsObjectHeld())
        {
            heldObject.transform.position = pickupParent.transform.position;
            heldObject.transform.rotation = pickupParent.transform.rotation;
            heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public bool IsObjectHeld()
    {
        return heldObject != null;
    }

    public void DropHeldObject()
    {
        heldObject = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == heldObject)
        {
            Physics.IgnoreCollision(heldObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
