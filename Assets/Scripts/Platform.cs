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

        // Setting the movement state to stopped
        currentMovementState = MovingStates.Stopped;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Checking if all buttons are pressed
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
                    // If all buttons are pressed, then the movement state is set to forward/backward depending on the current traversal point
                    currentMovementState = (currentPointIndex <= 0 ? MovingStates.Forward : MovingStates.Backward);
                    break;
                case MovingStates.Forward:
                    // If all buttons are pressed and the platform is moving forward it is reversed
                    currentMovementState = MovingStates.Backward;
                    currentPointIndex += 1;
                    break;
                case MovingStates.Backward:
                    // Inverse of above
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
                // If the platform is at the final movement point, it is stopped and snapped to that final point
                if (currentPointIndex >= movementPoints.Count - 1)
                {
                    currentMovementState = MovingStates.Stopped;
                    currentPointIndex = movementPoints.Count - 1;
                    platform.transform.position = movementPoints[movementPoints.Count - 1].position;
                    break;
                }

                // Getting the movement point target and creating a movement vector towards that target
                currentPointTarget = movementPoints[currentPointIndex];
                movementVector = currentPointTarget.forward * movementSpeed * Time.deltaTime;

                // Moving the platform towards that target using the caluclated vector above
                platform.transform.Translate(movementVector);

                // if the platform and the next movement point are close enough, the platform is snapped to that point and the next point is chosen
                if (Vector3.Distance(platform.transform.position, movementPoints[currentPointIndex + 1].position) <= platformSnapDistance)
                {
                    currentPointIndex += 1;
                    platform.transform.position = movementPoints[currentPointIndex].position;
                }

                break;

            case MovingStates.Backward:
                // If the current movement point is the first, then it is stopped and snapped to that point
                if (currentPointIndex <= 0)
                {
                    currentMovementState = MovingStates.Stopped;
                    currentPointIndex = 0;
                    platform.transform.position = movementPoints[0].position;
                    break;
                }

                // Calculating the movement vector needed to move to the next movement point
                currentPointTarget = movementPoints[currentPointIndex - 1];
                movementVector = (-currentPointTarget.forward) * movementSpeed * Time.deltaTime;

                // Moving the platform using the calculated vector above
                platform.transform.Translate(movementVector);
                
                // if the platform and next movement point are close enough, the platform is snapped to that point and the next point is chosen
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
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When a rigidbody collides with the platform, then it is stuck to the platform
        collision.gameObject.transform.SetParent(platform.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        // When a rigidbody leaves the platform's collider, it is removed from the platform as a child
        collision.gameObject.transform.SetParent(null);
    }
}
