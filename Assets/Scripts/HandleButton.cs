using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private bool isDesignPhase;
    private bool isMaterialPhase;
    private bool isColourPhase;
    private string[] values;
    private float spawnTimer;

    [SerializeField]
    private Transform[] phasePositions;

    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private Color[] colours;

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

        isDesignPhase = true;
        isMaterialPhase = false;
        isColourPhase = false;
        spawnTimer = 0;
    }

    void Update()
    {
        // Get the received message from ArduinoValueReader script
        string message = arduinoValueReader.GetCurrentMessage();
        spawnTimer += Time.deltaTime;

        if (!string.IsNullOrEmpty(message))
        {
            values = message.Split(',');
            HandleButtonPress(message);
        }
        HandleButtonPressNoArduino();
        if (isDesignPhase)
        {
            transform.position = Vector3.Lerp(transform.position, phasePositions[0].position, 0.6f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, phasePositions[0].rotation, 0.6f * Time.deltaTime);
            if (lastClickedObject != null)
            {
                if (values != null)
                {
                    // Update the rotation of the last clicked object
                    if (values[0] == "1") lastClickedObject.transform.Rotate(Vector3.up * objectRotationSpeed * Time.deltaTime, Space.World);
                    if (values[0] == "2") lastClickedObject.transform.Rotate(Vector3.up * -objectRotationSpeed * Time.deltaTime, Space.World);
                    if (values[1] == "1") lastClickedObject.transform.Rotate(Vector3.right * objectRotationSpeed * Time.deltaTime, Space.World);
                    if (values[1] == "2") lastClickedObject.transform.Rotate(Vector3.right * -objectRotationSpeed * Time.deltaTime, Space.World);
                }

                if (Input.GetKey(KeyCode.UpArrow)) lastClickedObject.transform.Rotate(Vector3.up * objectRotationSpeed * Time.deltaTime, Space.World);
                if (Input.GetKey(KeyCode.DownArrow)) lastClickedObject.transform.Rotate(Vector3.up * -objectRotationSpeed * Time.deltaTime, Space.World);
                if (Input.GetKey(KeyCode.LeftArrow)) lastClickedObject.transform.Rotate(Vector3.right * objectRotationSpeed * Time.deltaTime, Space.World);
                if (Input.GetKey(KeyCode.RightArrow)) lastClickedObject.transform.Rotate(Vector3.right * -objectRotationSpeed * Time.deltaTime, Space.World);
            }
        }
        if (isMaterialPhase)
        {
            transform.position = Vector3.Lerp(transform.position, phasePositions[1].position, 0.6f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, phasePositions[1].rotation, 0.6f * Time.deltaTime);
        }

        if (isColourPhase)
        {
            transform.position = Vector3.Lerp(transform.position, phasePositions[2].position, 0.6f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, phasePositions[2].rotation, 0.6f * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDesignPhase)
            {
                isDesignPhase = false;
                isMaterialPhase = true;
            }
            else if (isMaterialPhase)
            {
                isMaterialPhase = false;
                isColourPhase = true;
            }
            else if (isColourPhase)
            {
                isColourPhase = false;
                isDesignPhase = true;
            }
        }
    }


    void HandleButtonPress(string message)
    {
        // Check button states on pins 7 to 11
        if (values[3] == "0") // Button on pin 8 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y + mouseSpeed));
        }
        if (values[2] == "0") // Button on pin 7 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x + mouseSpeed, Input.mousePosition.y));
        }
        if (values[5] == "0") // Button on pin 10 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y - mouseSpeed));
        }
        if (values[4] == "0") // Button on pin 9 pressed
        {
            Mouse.current.WarpCursorPosition(new Vector2(Input.mousePosition.x - mouseSpeed, Input.mousePosition.y));
        }
        if (values[6] == "0") // Button on pin 11 pressed
        {
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);

            lastClickedObject = CheckForObjectClick();
        }


        if (isDesignPhase)
        {
            bool[] buttonStates = new bool[objectsToInstantiate.Length];

            for (int i = 0; i < objectsToInstantiate.Length - 1; i++)
            {
                buttonStates[i] = int.Parse(values[i + 9]) == 1;

                // Convert the mouse position to a world point
                Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, spawnRange));

                // Instantiate object on button press if the button was not pressed in the last frame
                if (buttonStates[i] && spawnTimer > 3)
                {
                    Instantiate(objectsToInstantiate[i], spawnPosition, Quaternion.identity);
                    spawnTimer = 0;
                }

                // Update the buttonPressedLastFrame array for the next frame
                buttonPressedLastFrame[i] = buttonStates[i];
            }
        }

        if (isMaterialPhase)
        {
            if (lastClickedObject != null)
            {
                bool[] buttonStates = new bool[materials.Length];

                for (int i = 0; i < materials.Length - 1; i++)
                {
                    buttonStates[i] = int.Parse(values[i + 9]) == 1;

                    // Instantiate object on button press if the button was not pressed in the last frame
                    if (buttonStates[i] && !buttonPressedLastFrame[i])
                    {
                        lastClickedObject.GetComponent<Renderer>().material = materials[i];
                    }

                    // Update the buttonPressedLastFrame array for the next frame
                    buttonPressedLastFrame[i] = buttonStates[i];
                }
            }
        }

        if (isColourPhase)
        {
            if (lastClickedObject != null)
            {
                bool[] buttonStates = new bool[colours.Length];

                for (int i = 0; i < colours.Length - 1; i++)
                {
                    buttonStates[i] = int.Parse(values[i + 9]) == 1;

                    // Instantiate object on button press if the button was not pressed in the last frame
                    if (buttonStates[i] && !buttonPressedLastFrame[i])
                    {
                        lastClickedObject.GetComponent<Renderer>().material.color = colours[i];
                    }

                    // Update the buttonPressedLastFrame array for the next frame
                    buttonPressedLastFrame[i] = buttonStates[i];
                }
            }
        }
    }

    void HandleButtonPressNoArduino()
    {
        if (Input.GetMouseButton(0)) // Button on pin 11 pressed
        {

            lastClickedObject = CheckForObjectClick();
        }


        if (isDesignPhase)
        {
            // Check keyboard input for object instantiation
            Vector3 spawnP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, spawnRange));
            if (Input.GetKeyDown(KeyCode.Alpha1)) Instantiate(objectsToInstantiate[0], spawnP, Quaternion.identity);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Instantiate(objectsToInstantiate[1], spawnP, Quaternion.identity);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Instantiate(objectsToInstantiate[2], spawnP, Quaternion.identity);
            if (Input.GetKeyDown(KeyCode.Alpha4)) Instantiate(objectsToInstantiate[3], spawnP, Quaternion.identity);
        }

        if (isMaterialPhase)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1)) lastClickedObject.GetComponent<Renderer>().material = materials[0];
            if (Input.GetKeyDown(KeyCode.Alpha2)) lastClickedObject.GetComponent<Renderer>().material = materials[1];
            if (Input.GetKeyDown(KeyCode.Alpha3)) lastClickedObject.GetComponent<Renderer>().material = materials[2];
            if (Input.GetKeyDown(KeyCode.Alpha4)) lastClickedObject.GetComponent<Renderer>().material = materials[3];
            if (Input.GetKeyDown(KeyCode.Alpha5)) lastClickedObject.GetComponent<Renderer>().material = materials[4];
            if (Input.GetKeyDown(KeyCode.Alpha6)) lastClickedObject.GetComponent<Renderer>().material = materials[5];
            if (Input.GetKeyDown(KeyCode.Alpha7)) lastClickedObject.GetComponent<Renderer>().material = materials[6];
            if (Input.GetKeyDown(KeyCode.Alpha8)) lastClickedObject.GetComponent<Renderer>().material = materials[7];
            if (Input.GetKeyDown(KeyCode.Alpha9)) lastClickedObject.GetComponent<Renderer>().material = materials[8];
        }

        if (isColourPhase)
        {


            if (Input.GetKeyDown(KeyCode.Alpha1)) lastClickedObject.GetComponent<Renderer>().material.color = colours[0];
            if (Input.GetKeyDown(KeyCode.Alpha2)) lastClickedObject.GetComponent<Renderer>().material.color = colours[1];
            if (Input.GetKeyDown(KeyCode.Alpha3)) lastClickedObject.GetComponent<Renderer>().material.color = colours[2];
            if (Input.GetKeyDown(KeyCode.Alpha4)) lastClickedObject.GetComponent<Renderer>().material.color = colours[3];
            if (Input.GetKeyDown(KeyCode.Alpha5)) lastClickedObject.GetComponent<Renderer>().material.color = colours[4];
            if (Input.GetKeyDown(KeyCode.Alpha6)) lastClickedObject.GetComponent<Renderer>().material.color = colours[5];
            if (Input.GetKeyDown(KeyCode.Alpha7)) lastClickedObject.GetComponent<Renderer>().material.color = colours[6];
            if (Input.GetKeyDown(KeyCode.Alpha8)) lastClickedObject.GetComponent<Renderer>().material.color = colours[7];
            if (Input.GetKeyDown(KeyCode.Alpha9)) lastClickedObject.GetComponent<Renderer>().material.color = colours[8];


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
            Debug.Log("No values received yet.");
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
