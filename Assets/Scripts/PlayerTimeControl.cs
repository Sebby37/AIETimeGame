using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeControl : MonoBehaviour
{
    // A list of all TimeBehaviour scripts in objects in the scene
    private List<TimeBehaviour> timeBehaviours = new List<TimeBehaviour>();

    // An enum of the TimeBehaviour functions which makes controlling them easier
    private enum TimeBehaviourFunctions
    {
        SlowTime,
        SpeedTime,
        StopTime,
        ResumeTime
    }
    // Start is called before the first frame update
    void Start()
    {
        // Populating the list of TimeBehaviours
        foreach(Object obj in Resources.FindObjectsOfTypeAll(typeof(TimeBehaviour)))
            if(obj != null)
                timeBehaviours.Add(obj as TimeBehaviour);
    }

    // Update is called once per frame
    void Update()
    {
        // Stopping and resuming time based on the key up/down events
        if (Input.GetKeyDown(KeyCode.Mouse1))
            CallFunctionInObjects(TimeBehaviourFunctions.StopTime);
        else if (Input.GetKeyUp(KeyCode.Mouse1))
            CallFunctionInObjects(TimeBehaviourFunctions.ResumeTime);

        // Same as above
        else if (Input.GetKeyDown(KeyCode.Q))
            CallFunctionInObjects(TimeBehaviourFunctions.StopTime);
        else if (Input.GetKeyUp(KeyCode.Q))
            CallFunctionInObjects(TimeBehaviourFunctions.ResumeTime);
    }

    // Function to call a function in all TimeBehaviour scripts
    void CallFunctionInObjects(TimeBehaviourFunctions functionName)
    {
        // Looping through each script and calling the given function based on a switch statements
        foreach (TimeBehaviour timeBehaviour in timeBehaviours)
        {
            switch (functionName)
            {
                case TimeBehaviourFunctions.SlowTime:
                    timeBehaviour.SlowTime();
                    break;
                case TimeBehaviourFunctions.SpeedTime:
                    timeBehaviour.SpeedTime();
                    break;
                case TimeBehaviourFunctions.StopTime:
                    timeBehaviour.StopTime();
                    break;
                case TimeBehaviourFunctions.ResumeTime:
                    timeBehaviour.ResumeTime();
                    break;
                default:
                    break;
            }
        }
    }
}
