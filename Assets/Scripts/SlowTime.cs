using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    public bool canSlowTime = true;

    [Range(0f, 1f)]
    public float slowSpeed = 0.25f;

    public static float scale = 1;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: set the rigidbody motion of objects to use this version of scale instead
        //*Time.deltaTime * SlowTime.scale;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSlowTime)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                Time.timeScale = slowSpeed;
            }
            else
            {
                Time.timeScale = 1;
            }
            // Use Time.unscaledDeltaTime to move the player while timescale is 0
        }
    }
}
