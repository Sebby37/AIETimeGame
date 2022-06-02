using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public float pushForce = 5;
    public float maxPushDistance = 3;
    public Transform pushPoint;

    private PickupObject pickupObject;
    // Start is called before the first frame update
    void Start()
    {
        pickupObject = GetComponent<PickupObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(pushPoint.position, pushPoint.forward * maxPushDistance, Color.blue);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            if (Physics.Raycast(pushPoint.position, pushPoint.forward, out hit, maxPushDistance))
            {
                GameObject hitObject;
                Rigidbody hitRb;

                hitObject = hit.collider.gameObject;
                hitRb = hitObject.GetComponent<Rigidbody>();

                if (pickupObject.IsObjectHeld())
                    pickupObject.DropHeldObject();

                if (hitRb != null)
                    hitRb.AddForce(pushPoint.forward * pushForce, ForceMode.Impulse);
            }
        }
    }
}
