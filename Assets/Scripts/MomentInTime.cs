using UnityEngine;
public struct MomentInTime
{
    public Transform position;
    public Quaternion rotation;
    public Rigidbody rb;

    public MomentInTime(Transform position, Quaternion rotation, Rigidbody rb)
    {
        this.position = position;
        this.rotation = rotation;
        this.rb = rb;
    }
}
