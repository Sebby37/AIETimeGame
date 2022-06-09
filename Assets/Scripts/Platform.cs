using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Platform GameObjects")]
    public GameObject platform;
    public List<Transform> movementPoints = new List<Transform>();

    [Header("Platform Parameters")]
    public float movementSpeed = 2.5f;

    [Header("Trigger Parameters")]
    public List<Button> buttonTriggers = new List<Button>();

    [Header("Temp stuff")]
    public float upSpeed;
    public float changeSpeed;

    public enum MovingStates
    {
        Stopped,
        Forward,
        Backward
    }
    public MovingStates currentMovementState;

    // Private variables
    private int currentPointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Adding the platform's starting position as the first transform point
        GameObject initialPosition = new GameObject("Initial Platform Position");
        initialPosition.transform.position = transform.position;
        movementPoints.Insert(0, initialPosition.transform);

        // Creating a path of spheres showing the path of the platform
        CreatePlatformPath(3);

        currentMovementState = MovingStates.Stopped;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.position += Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * changeSpeed) * upSpeed;
        bool allButtonsPressed = true;
        foreach (Button button in buttonTriggers)
            if (!button.IsPressed())
                allButtonsPressed = false;

        // Handling button press events
        if (allButtonsPressed)
        {
            switch (currentMovementState)
            {
                case MovingStates.Stopped:
                    currentMovementState = MovingStates.Forward;
                    break;
                case MovingStates.Forward:
                    currentMovementState = MovingStates.Backward;
                    break;
                case MovingStates.Backward:
                    currentMovementState = MovingStates.Forward;
                    break;
            }

            // Unpressing all pushable buttons
            foreach (Button button in buttonTriggers)
                if (button.buttonType == Button.ButtonTypes.Pressable)
                    button.UnPress();
        }

        // Moving the platform
        switch (currentMovementState)
        {
            case MovingStates.Forward:
                if (currentPointIndex >= movementPoints.Count)
                {
                    currentMovementState = MovingStates.Stopped;
                    break;
                }

                Debug.Log($"{platform.transform.position} {movementPoints[currentPointIndex + 1].position} {currentPointIndex} {platform.transform.position == movementPoints[currentPointIndex + 1].position}");

                Transform currentPointTarget = movementPoints[currentPointIndex];
                Vector3 movementVector = currentPointTarget.forward * movementSpeed * Time.deltaTime;

                platform.transform.Translate(movementVector);

                // TODO: Make this work with the final position I guess? IDK just run the code and look at the bug on the platform

                if (platform.transform.position == movementPoints[currentPointIndex + 1].position)// && platform.transform.position != movementPoints[0].position)
                {
                    currentPointIndex += 1;
                    Debug.Log("Next position");
                }

                break;
        }
    }

    void CreatePlatformPath(int spheresPerPoint)
    {
        for (int i = 0; i < movementPoints.Count; i++)
        {
            if (i + 1 >= movementPoints.Count) break;

            Transform currentPoint = movementPoints[i].transform;
            Transform nextPoint = movementPoints[i + 1].transform;
            float distanceBetweenPoints = Vector3.Distance(currentPoint.position, nextPoint.position);
            float distanceBetweenSpherePoints = distanceBetweenPoints / spheresPerPoint;

            for (int j = 0; j < spheresPerPoint; j++)
            {
                GameObject currentSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                currentSphere.GetComponent<Collider>().enabled = false;
                currentSphere.transform.SetParent(transform, false);
                currentSphere.transform.position = currentPoint.position + (currentPoint.transform.forward * distanceBetweenSpherePoints * j);
                currentSphere.transform.localScale = Vector3.one / 4;
            }
        }
    }
}
