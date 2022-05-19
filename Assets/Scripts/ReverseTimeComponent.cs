using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseTimeComponent : MonoBehaviour
{
    // Public variables
    public bool rewinding = false;
    public static int maxPointsInTime = 100;
    public static float timeBetweenRecord = 0.1f;
    
    // Private variables
    private Rigidbody rb;
    private List<MomentInTime> momentsInTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        momentsInTime = new List<MomentInTime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
