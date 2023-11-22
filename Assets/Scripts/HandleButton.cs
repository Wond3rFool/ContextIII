using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleButton : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToInstantiate1; // Reference to the prefab you want to instantiate
    [SerializeField]
    private GameObject prefabToInstantiate2; // Reference to the prefab you want to instantiate
    [SerializeField]
    private GameObject prefabToInstantiate3; // Reference to the prefab you want to instantiate
    [SerializeField]
    private GameObject prefabToInstantiate4; // Reference to the prefab you want to instantiate
    [SerializeField]
    private GameObject prefabToInstantiate5; // Reference to the prefab you want to instantiate

    // Reference to the ArduinoValueReader script
    public ArduinoValueReader arduinoValueReader;

    public float mouseSpeed = 5;

    private bool canSpawn;

    void Start()
    {
        // Ensure that the ArduinoValueReader script is assigned
        if (arduinoValueReader == null)
        {
            Debug.LogError("ArduinoValueReader script not assigned to HandleButton script.");
        }
        canSpawn = true;
    }

    void Update()
    {
        // Get the received message from ArduinoValueReader script
        string message = arduinoValueReader.GetCurrentMessage();

        if (!string.IsNullOrEmpty(message))
        {
            HandleButtonPress(message);
            Debug.Log(message);
        }
    }

    void HandleButtonPress(string message)
    {
        // Parse the button number from the received message
        if (int.TryParse(message, out int buttonNumber))
        {
            if (canSpawn) 
            {
                switch (buttonNumber)
                {
                    case 0:
                        canSpawn = true;
                        break;
                    case 1:
                        Instantiate(prefabToInstantiate1, new Vector3(0, 0, 15), Quaternion.identity);
                        canSpawn = false;
                        break;
                    case 2:
                        Instantiate(prefabToInstantiate2, new Vector3(0, 0, 15), Quaternion.identity);
                        canSpawn = false;
                        break;
                    case 3:
                        Instantiate(prefabToInstantiate3, new Vector3(0, 0, 15), Quaternion.identity);
                        canSpawn = false;
                        break;
                    case 4:
                        Instantiate(prefabToInstantiate4, new Vector3(0, 0, 15), Quaternion.identity);
                        canSpawn = false;
                        break;
                    case 5:
                        Instantiate(prefabToInstantiate5, new Vector3(0, 0, 15), Quaternion.identity);
                        canSpawn = false;
                        break;
                }
            }
            // Check if the button number is within the expected range

            if (buttonNumber == 6)
            {
                Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y + mouseSpeed));
            }
            if (buttonNumber == 7)
            {
                Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x + mouseSpeed, Input.mousePosition.y));
            }
            if (buttonNumber == 8)
            {
                Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y - mouseSpeed));
            }
            if (buttonNumber == 9)
            {
                Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x - mouseSpeed, Input.mousePosition.y));
            }
            if (buttonNumber == 10)
            {
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
                Debug.Log("test");
            }
        }
        else
        {
            Debug.LogWarning("Failed to parse the button number from the received message.");
        }
    }
}
