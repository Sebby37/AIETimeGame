using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBehaviour : MonoBehaviour
{
    // Public variables
    public TimeStates state;
    public bool affectedByStop = true;
    public float slowTimeScale = 0.5f;
    public bool affectedBySlowdown = true;

    // Private variables
    private Rigidbody rb;
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
                //rb.velocity = rb.velocity / (1 - Time.deltaTime * rb.drag);
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
        rb.drag = 5 / slowTimeScale;  // TODO: Scale downwards acceleration so you fall naturally
        rb.angularDrag = 5 / slowTimeScale;
        //Physics.gravity *= slowTimeScale;
    }

    public void SpeedTime()
    {
        if (!affectedBySlowdown) return;

        state = TimeStates.Normal;
        rb.velocity = rb.velocity / (1 - Time.deltaTime * rb.drag);

        rb.drag = 0;
        rb.angularDrag = 0.05f;
        //Physics.gravity /= slowTimeScale;
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
