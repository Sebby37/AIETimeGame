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
    [Range(0f, 1f)]
    public float platformSnapDistance = 0.1f;

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

    /*
    TODO:
    - Use parenting to attach and detach the player to the platform when they are inside a trigger collider
    - Test the platform with weighted buttons
    */

    // Start is called before the first frame update
    void Start()
    {
        // Adding the platform's starting position as the first transform point
        GameObject initialPosition = new GameObject("Initial Platform Position");
        initialPosition.transform.SetParent(transform);
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
                    currentMovementState = (currentPointIndex <= 0 ? MovingStates.Forward : MovingStates.Backward);
                    break;
                case MovingStates.Forward:
                    currentMovementState = MovingStates.Backward;
                    currentPointIndex += 1;
                    break;
                case MovingStates.Backward:
                    currentMovementState = MovingStates.Forward;
                    currentPointIndex -= 1;
                    break;
            }

            // Unpressing all pushable buttons
            foreach (Button button in buttonTriggers)
                if (button.buttonType == Button.ButtonTypes.Pressable)
                    button.UnPress();
        }

        // Moving the platform
        Vector3 movementVector;
        Transform currentPointTarget;
        switch (currentMovementState)
        {
            case MovingStates.Forward:
                if (currentPointIndex >= movementPoints.Count - 1)
                {
                    currentMovementState = MovingStates.Stopped;
                    currentPointIndex = movementPoints.Count - 1;
                    platform.transform.position = movementPoints[movementPoints.Count - 1].position;
                    break;
                }

                currentPointTarget = movementPoints[currentPointIndex];
                movementVector = currentPointTarget.forward * movementSpeed * Time.deltaTime;

                platform.transform.Translate(movementVector);

                if (Vector3.Distance(platform.transform.position, movementPoints[currentPointIndex + 1].position) <= platformSnapDistance)
                {
                    currentPointIndex += 1;
                    platform.transform.position = movementPoints[currentPointIndex].position;
                }

                break;

            case MovingStates.Backward:
                if (currentPointIndex <= 0)
                {
                    currentMovementState = MovingStates.Stopped;
                    currentPointIndex = 0;
                    platform.transform.position = movementPoints[0].position;
                    break;
                }

                currentPointTarget = movementPoints[currentPointIndex - 1];
                movementVector = (-currentPointTarget.forward) * movementSpeed * Time.deltaTime;

                platform.transform.Translate(movementVector);
                Debug.Log($"{movementPoints[currentPointIndex - 1].gameObject.name} {platform.transform.position} {movementPoints[currentPointIndex - 1].position} {Vector3.Distance(platform.transform.position, movementPoints[currentPointIndex - 1].position)}");
                if (Vector3.Distance(platform.transform.position, movementPoints[currentPointIndex - 1].position) <= platformSnapDistance)
                {
                    currentPointIndex -= 1;
                    platform.transform.position = movementPoints[currentPointIndex].position;
                }

                break;
        }
    }

    // Function to create a path of spheres that shows the path a platform will traverse
    void CreatePlatformPath(int spheresPerPoint)
    {
        // Creating a parent object fot the spheres to make the list of gameobjects easier to read
        GameObject sphereParent = new GameObject("Movement Path Spheres");
        sphereParent.transform.SetParent(transform);
        
        // Looping through each movement point for the platform
        for (int i = 0; i < movementPoints.Count; i++)
        {
            // If there is no next movement point, then no more spheres need to be created and the loop is broken
            if (i + 1 >= movementPoints.Count) break;

            // Getting the current and next movement points
            Transform currentPoint = movementPoints[i].transform;
            Transform nextPoint = movementPoints[i + 1].transform;

            // Getting the distance between the points and calculating the distance the sphere points must be from each other
            float distanceBetweenPoints = Vector3.Distance(currentPoint.position, nextPoint.position);
            float distanceBetweenSpherePoints = distanceBetweenPoints / spheresPerPoint;

            // Making the current point look towards the next point to make working with vectors easier
            currentPoint.LookAt(nextPoint);

            for (int j = 0; j <= spheresPerPoint; j++)
            {
                // Creating a sphere and setting it's name to something identifiable
                GameObject currentSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                currentSphere.name = "Movement Path Sphere";

                // Disabling the sphere's collider and setting it's parent to the sphere parent created earlier
                currentSphere.GetComponent<Collider>().enabled = false;
                currentSphere.transform.SetParent(sphereParent.transform, false);

                // Calculating where the current sphere should appear along the path and scaling it down
                currentSphere.transform.position = currentPoint.position + (currentPoint.transform.forward * distanceBetweenSpherePoints * j);
                currentSphere.transform.localScale = Vector3.one / 4;

                // If there is a sphere close to the a point, it is destroyed
                //if (Vector3.Distance(currentSphere.transform.position, currentPoint.position) < distanceBetweenSpherePoints) Destroy(currentSphere);
            }
        }
    }
}
