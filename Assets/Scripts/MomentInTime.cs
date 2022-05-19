using UnityEngine;
public struct MomentInTime
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public MomentInTime(Transform transform, Rigidbody rb)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
        this.velocity = rb.velocity;
        this.angularVelocity = rb.angularVelocity;
    }
}
