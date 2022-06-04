using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour
{
    [Header("Spawner Parameters")]
    public GameObject cube;
    public Transform spawnTransform;

    [Header("Spawned Object Parameters")]
    public bool spawnOnlyOne = true;

    [Header("Trigger Parameters")]
    public List<Button> buttonTriggers = new List<Button>();

    private GameObject spawnedCube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool canSpawnCube = true;
        foreach (Button button in buttonTriggers)
            if (!button.IsPressed())
                canSpawnCube = false;

        if (canSpawnCube)
        {
            if (spawnOnlyOne) 
                Destroy(spawnedCube);

            spawnedCube = Instantiate(cube, spawnTransform.position, Quaternion.identity);
        }

        // Unpressing all pushable buttons
        foreach (Button button in buttonTriggers)
            if (button.buttonType == Button.ButtonTypes.Pressable)
                button.UnPress();
    }
}
