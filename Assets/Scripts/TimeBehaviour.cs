using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBehaviour : MonoBehaviour
{
    // Public variables
    public TimeStates state;
    public bool affectedByStop = true;
    public bool affectedBySlowdown = true;

    // Private variables
    private Rigidbody rb;
    private float slowTimeScale = 0.25f;
    private Vector3 previousVelocity;
    private Vector3 previousAngularVelocity;

    // Enums
    public enum TimeStates
    {
        Normal,
        Slow,
        Stopped
    }
    // Start is called before the first frame update
    void Start()
    {
        state = TimeStates.Normal;
        rb = GetComponent<Rigidbody>();
    }
    /*
    Notes:
        - I cannot use rb.isKinematic, as then I won't be able to pick up/move objects in stopped time
        - To store angular and tangential velocity, I'll use Vector3s, if the velocity in stopped time is altered then angular velocity is set to zero
        - I'll have to find some sort of workaround when working with velocity in stopped time, as I'll be unable to move it if it is set to zero
        - I'm considering changing stopped time so you can jump on stopped objects and click at them to apply a force
        - READ COMMENT ON SPEED TIME AND WORK ON IT WITH FIXEDUPDATE TOO
    */

    // Update is called once per frame
    void FixedUpdate()
    {

        switch (state)
        {
            case TimeStates.Slow:
                // Creating a temporary velocity vector to apply motion to
                Vector3 currentVelocity = rb.velocity;

                currentVelocity.x *= slowTimeScale;
                currentVelocity.y *= slowTimeScale;
                currentVelocity.z *= slowTimeScale;

                rb.velocity = currentVelocity;
                rb.angularVelocity *= slowTimeScale;
                break;
            case TimeStates.Stopped:
                rb.isKinematic = true;
                break;
            default:
                break;
        }
    }

    public void SlowTime()
    {
        if (!affectedBySlowdown) return;

        state = TimeStates.Slow;
    }

    public void SpeedTime()
    {
        if (!affectedBySlowdown) return;

        // Performing the inverse of the velocity operations in the FixedUpdate function to revert the velocities back to a normal scale
        Vector3 currentVelocity = rb.velocity;

        // TODO: Fix this for physics object such as the box, where movement is halted significantly more than the player
        currentVelocity.x /= slowTimeScale;
        currentVelocity.y /= slowTimeScale;
        currentVelocity.z /= slowTimeScale;

        rb.velocity = currentVelocity;
        rb.angularVelocity /= slowTimeScale;

        state = TimeStates.Normal;
    }

    public void StopTime()
    {
        if (!affectedByStop) return;

        previousVelocity = rb.velocity;
        previousAngularVelocity = rb.angularVelocity;

        /*rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;*/
        rb.isKinematic = true;

        state = TimeStates.Stopped;
    }

    public void ResumeTime()
    {
        if (!affectedByStop) return;

        rb.isKinematic = false;

        rb.velocity = previousVelocity;
        rb.angularVelocity = previousAngularVelocity;
        rb.useGravity = true;

        state = TimeStates.Normal;
    }
}
