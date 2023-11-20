using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // Ensure that the ArduinoValueReader script is assigned
        if (arduinoValueReader == null)
        {
            Debug.LogError("ArduinoValueReader script not assigned to HandleButton script.");
        }
    }

    void Update()
    {
        // Get the received message from ArduinoValueReader script
        string message = arduinoValueReader.GetCurrentMessage();

        if (!string.IsNullOrEmpty(message))
        {
            HandleButtonPress(message);
        }
    }

    void HandleButtonPress(string message)
    {
        // Parse the button number from the received message
        if (int.TryParse(message, out int buttonNumber))
        {
            // Check if the button number is within the expected range
            if (buttonNumber >= 1 && buttonNumber <= 5)
            {
                // Handle button presses with use cases
                switch (buttonNumber)
                {
                    case 1:
                        Instantiate(prefabToInstantiate1, new Vector3(0, 0, 15), Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(prefabToInstantiate2, new Vector3(0, 0, 15), Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(prefabToInstantiate3, new Vector3(0, 0, 15), Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(prefabToInstantiate4, new Vector3(0, 0, 15), Quaternion.identity);
                        break;
                    case 5:
                        Instantiate(prefabToInstantiate5, new Vector3(0, 0, 15), Quaternion.identity);
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Received an invalid button number: " + buttonNumber);
            }
        }
        else
        {
            Debug.LogWarning("Failed to parse the button number from the received message.");
        }
    }
}
