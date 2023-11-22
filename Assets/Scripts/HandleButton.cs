using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleButton : MonoBehaviour
{
    [SerializeField]
    public GameObject[] objectsToInstantiate;

    // Set the position where you want to instantiate the objects
    public Transform instantiatePosition;

    // Reference to the ArduinoValueReader script
    public ArduinoValueReader arduinoValueReader;

    // Array to store the button states in the previous frame
    private bool[] buttonPressedLastFrame;

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
        string[] values = message.Split(',');
        Debug.Log(values.Length);
        int joystickX = int.Parse(values[0]);
        int joystickY = int.Parse(values[1]);
        int joystickButton = int.Parse(values[7]);

        if (joystickX == 1)
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y + mouseSpeed));
        }
        if (joystickY == 1)
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x + mouseSpeed, Input.mousePosition.y));
        }
        if (joystickX == 2)
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y - mouseSpeed));
        }
        if (joystickY == 2)
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x - mouseSpeed, Input.mousePosition.y));
        }
        if (joystickButton == 0)
        {
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
            Debug.Log("test");
        }


        bool[] buttonStates = new bool[objectsToInstantiate.Length];

        for (int i = 0; i < objectsToInstantiate.Length-1; i++)
        {
            buttonStates[i] = int.Parse(values[i + 2]) == 0;

            // Instantiate object on button press if the button was not pressed in the last frame
            if (buttonStates[i] && !buttonPressedLastFrame[i])
            {
                Instantiate(objectsToInstantiate[i], instantiatePosition.position, instantiatePosition.rotation);
            }

            // Update the buttonPressedLastFrame array for the next frame
            buttonPressedLastFrame[i] = buttonStates[i];
        }

        // Adjust object movement based on joystick input
        //Vector3 movement = new Vector3(joystickX, 0, joystickY) * moveSpeed * Time.deltaTime;
        //transform.Translate(movement);
    }
}

