using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleButton : MonoBehaviour
{
    [SerializeField]
    public GameObject[] objectsToInstantiate;

    // Reference to the ArduinoValueReader script
    public ArduinoValueReader arduinoValueReader;

    public int spawnRange;
    private Vector3 mousePosition;
    // Array to store the button states in the previous frame
    private bool[] buttonPressedLastFrame;
    private string[] values;

    public float sensitivity = 5;
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

        // Initialize the buttonPressedLastFrame array
        buttonPressedLastFrame = new bool[objectsToInstantiate.Length];
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
        values = message.Split(',');
        Debug.Log(values.Length);

        // Check button states on pins 7 to 11
        if (values[8] == "0") // Button on pin 8 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y + mouseSpeed));
        }
        if (values[7] == "0") // Button on pin 7 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x + mouseSpeed, Input.mousePosition.y));
        }
        if (values[10] == "0") // Button on pin 10 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y - mouseSpeed));
        }
        if (values[9] == "0") // Button on pin 9 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x - mouseSpeed, Input.mousePosition.y));
        }
        if (values[11] == "0") // Button on pin 11 pressed
        {
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
            Debug.Log("test");
        }

        bool[] buttonStates = new bool[objectsToInstantiate.Length];

        for (int i = 0; i < objectsToInstantiate.Length - 1; i++)
        {
            buttonStates[i] = int.Parse(values[i + 2]) == 0;
            mousePosition = Input.mousePosition;

            // Convert the mouse position to a world point
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, spawnRange));
            // Instantiate object on button press if the button was not pressed in the last frame
            if (buttonStates[i] && !buttonPressedLastFrame[i])
            {
                Instantiate(objectsToInstantiate[i], spawnPosition, Quaternion.identity);
            }

            // Update the buttonPressedLastFrame array for the next frame
            buttonPressedLastFrame[i] = buttonStates[i];
        }
    }

    public string[] GetValue()
    {
        if (values != null && values.Length >= 1)
        {
            return values;
        }
        else
        {
            Debug.LogError("No values received yet.");
            return null;
        }
    }
}
