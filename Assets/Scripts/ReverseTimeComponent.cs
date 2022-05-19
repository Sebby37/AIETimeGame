using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseTimeComponent : MonoBehaviour
{
    // Public variables
    public bool rewinding = false;
    public int maxPointsInTime = 100;
    //public float timeBetweenRecord = 0.1f;

    // Private variables
    private Rigidbody rb;
    private List<MomentInTime> momentsInTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        momentsInTime = new List<MomentInTime>();
    }

    private void FixedUpdate()
    {
        // Applying physics based on whether the object is rewinding
        rb.isKinematic = rewinding;

        // Rewinding the object or recording it's position based on whether the object is rewinding
        if (rewinding)
            Rewind();
        else
            Record();
    }

    // Update is called once per frame
    void Update()
    {
        // Currently for debugging purposes, will be in a game manager later for performance
        rewinding = Input.GetKey(KeyCode.R);
    }

    void Record()
    {
        // TODO: Add in a delay between recording
        // Removing the earliest moment in time when the maximum moments in time have been reached
        if (momentsInTime.Count >= maxPointsInTime)
            momentsInTime.RemoveAt(momentsInTime.Count - 1);

        // Adding the current transform and rigidbody as a moment in time to the list
        MomentInTime currentMoment = new MomentInTime(transform, rb);
        momentsInTime.Insert(0, currentMoment);
    }

    void Rewind()
    {
        if (momentsInTime.Count > 0)
        {
            // Getting the latest moment in time
            MomentInTime currentMoment = momentsInTime[0];

            // Setting the position and rotation of the object to that of the latest moment in time
            transform.position = currentMoment.position;
            transform.rotation = currentMoment.rotation;

            rb.isKinematic = true;

            // Setting the velocity and angular velocity of the object
            rb.velocity = currentMoment.velocity;
            rb.angularVelocity = currentMoment.angularVelocity;

            Debug.Log($"cur moment Velocity: {currentMoment.velocity}\nrigidbody Velocity: {rb.velocity}");

            // Removing the latest moment in time as it is no longer being used
            momentsInTime.RemoveAt(0);
        }
        else
        {
            rewinding = false;
        }
    }
}
