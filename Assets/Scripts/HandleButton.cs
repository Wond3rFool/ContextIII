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
    private GameObject lastClickedObject;
    // Array to store the button states in the previous frame
    private bool[] buttonPressedLastFrame;
    private string[] values;

    public float sensitivity = 5;
    public float mouseSpeed = 5;

    public float objectRotationSpeed;

    void Start()
    {
        // Ensure that the ArduinoValueReader script is assigned
        if (arduinoValueReader == null)
        {
            Debug.LogError("ArduinoValueReader script not assigned to HandleButton script.");
        }

        // Initialize the buttonPressedLastFrame array
        buttonPressedLastFrame = new bool[objectsToInstantiate.Length];
    }

    void Update()
    {
        // Get the received message from ArduinoValueReader script
        string message = arduinoValueReader.GetCurrentMessage();

        if (!string.IsNullOrEmpty(message))
        {
            values = message.Split(',');
            HandleButtonPress(message);
        }

        if (lastClickedObject != null && values.Length > 0)
        {
            Debug.Log(lastClickedObject);
            // Update the rotation of the last clicked object
            if (values[0] == "1") lastClickedObject.transform.Rotate(Vector3.up * objectRotationSpeed * Time.deltaTime, Space.World);
            if (values[0] == "2") lastClickedObject.transform.Rotate(Vector3.up * -objectRotationSpeed * Time.deltaTime, Space.World);
            if (values[1] == "1") lastClickedObject.transform.Rotate(Vector3.right * objectRotationSpeed * Time.deltaTime, Space.World);
            if (values[1] == "2") lastClickedObject.transform.Rotate(Vector3.right * -objectRotationSpeed * Time.deltaTime, Space.World);
        }
    }



    void HandleButtonPress(string message)
    {
        values = message.Split(',');

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

            lastClickedObject = CheckForObjectClick();
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

    GameObject CheckForObjectClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            ObjectDrag clickedObject = hit.collider.gameObject.GetComponent<ObjectDrag>();

            return clickedObject.gameObject; // Return the clicked object
        }

        return null; // Return null if no object is clicked
    }
}
