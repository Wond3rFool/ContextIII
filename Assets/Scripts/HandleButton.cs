using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleButton : MonoBehaviour
{
    [SerializeField]
    public GameObject[] objectsToInstantiate;

    public Material material;
    public Shader shaderOff;
    public Shader sketchShader;

    public AudioSource source;

    public GameObject UiTop;

    public AudioClip spawnItem;
    public AudioClip changeColour;
    public AudioClip switchPhase;
    public AudioClip itemSelect;
    public AudioClip itemDrop;

    // Reference to the ArduinoValueReader script
    public ArduinoValueReader arduinoValueReader;

    public GameObject domeObjects;

    public Material skyboxMaterial;

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
    private float mouseTimer;
    private bool CanRotate;
    private bool mouseInSamePos;

    private bool spawnItemSound;

    [SerializeField]
    private Transform[] phasePositions;

    private Vector3 OldMousePosition;

    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private Color[] colours;

    public float sensitivity = 5;
    public float mouseSpeed = 5;

    public float objectRotationSpeed;

    void Start()
    {
        spawnItemSound = false;
        // Ensure that the ArduinoValueReader script is assigned
        if (arduinoValueReader == null)
        {
            Debug.LogError("ArduinoValueReader script not assigned to HandleButton script.");
        }

        // Initialize the buttonPressedLastFrame array
        buttonPressedLastFrame = new bool[objectsToInstantiate.Length];

        isDesignPhase = true;
        mouseInSamePos = false;
        isMaterialPhase = false;
        CanRotate = true;
        domeObjects.SetActive(false);
        UiTop.SetActive(true);
        spawnTimer = 0;
    }

    void Update()
    {
        if (spawnItemSound)
        {
            source.PlayOneShot(spawnItem);
            spawnItemSound = false;
        }

        mousePosition = Input.mousePosition;
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
            domeObjects.SetActive(false);
            UiTop.SetActive(true);
            material.shader = sketchShader;
            RenderSettings.skybox = null;

            if (lastClickedObject != null)
            {
                if (values != null)
                {
                    // Update the rotation of the last clicked object
                    if (values[0] == "1") lastClickedObject.transform.Rotate(Vector3.up * -objectRotationSpeed * Time.deltaTime, Space.World);
                    if (values[0] == "2") lastClickedObject.transform.Rotate(Vector3.up * objectRotationSpeed * Time.deltaTime, Space.World);
                    if (values[1] == "1") lastClickedObject.transform.Rotate(Vector3.right * objectRotationSpeed * Time.deltaTime, Space.World);
                    if (values[1] == "2") lastClickedObject.transform.Rotate(Vector3.right * -objectRotationSpeed * Time.deltaTime, Space.World);
                }

                if (Input.GetKey(KeyCode.UpArrow)) lastClickedObject.transform.Rotate(Vector3.right * -objectRotationSpeed * Time.deltaTime, Space.World);
                if (Input.GetKey(KeyCode.DownArrow)) lastClickedObject.transform.Rotate(Vector3.right * objectRotationSpeed * Time.deltaTime, Space.World);
                if (Input.GetKey(KeyCode.LeftArrow)) lastClickedObject.transform.Rotate(Vector3.up * objectRotationSpeed * Time.deltaTime, Space.World);
                if (Input.GetKey(KeyCode.RightArrow)) lastClickedObject.transform.Rotate(Vector3.up * -objectRotationSpeed * Time.deltaTime, Space.World);
            }
        }
        if (isMaterialPhase)
        {
            domeObjects.SetActive(true);
            UiTop.SetActive(false);
            material.shader = shaderOff;
            RenderSettings.skybox = skyboxMaterial;
        }

        if (OldMousePosition == mousePosition)
        {
            mouseTimer += Time.deltaTime;
        }
        else
        {
            mouseInSamePos = false;
            mouseTimer = 0;
        }

        if (mouseTimer > 20)
        {
            mouseInSamePos = true;
        }
        OldMousePosition = mousePosition;
    }

    public void DeleteSelectedGameObject()
    {
        if (lastClickedObject != null)
        {
            Destroy(lastClickedObject.gameObject);
        }
    }

    void HandleButtonPress(string message)
    {
        if (isDesignPhase)
        {

            bool[] buttonStates = new bool[objectsToInstantiate.Length];

            for (int i = 0; i < objectsToInstantiate.Length; i++)
            {
                buttonStates[i] = int.Parse(values[i + 2]) == 1;

                // Convert the mouse position to a world point
                Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, spawnRange));

                // Instantiate object on button press if the button was not pressed in the last frame
                if (buttonStates[i] && spawnTimer > 3 && !mouseInSamePos)
                {
                    Instantiate(objectsToInstantiate[i], spawnPosition, Quaternion.identity);
                    spawnItemSound = true;
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

                for (int i = 0; i < materials.Length; i++)
                {
                    buttonStates[i] = int.Parse(values[i + 14]) == 1;

                    // Instantiate object on button press if the button was not pressed in the last frame
                    if (buttonStates[i] && !buttonPressedLastFrame[i])
                    {
                        foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                        {
                            childRenderer.material.color = colours[i];
                        }
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

            if (spawnTimer > 3 && !mouseInSamePos)
            {
                // Check keyboard input for object instantiation
                Vector3 spawnP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, spawnRange));
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Instantiate(objectsToInstantiate[0], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Instantiate(objectsToInstantiate[1], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Instantiate(objectsToInstantiate[2], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    Instantiate(objectsToInstantiate[3], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    Instantiate(objectsToInstantiate[4], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    Instantiate(objectsToInstantiate[5], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    Instantiate(objectsToInstantiate[6], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    Instantiate(objectsToInstantiate[7], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    Instantiate(objectsToInstantiate[8], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    Instantiate(objectsToInstantiate[9], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Minus))
                {
                    Instantiate(objectsToInstantiate[10], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }
                if (Input.GetKeyDown(KeyCode.Equals))
                {
                    Instantiate(objectsToInstantiate[11], spawnP, Quaternion.identity);
                    spawnItemSound = true;
                    spawnTimer = 0;
                }

            }
        }

        if (isMaterialPhase)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1)) 
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[0];
                }
            } 
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[1];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[2];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[3];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[4];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[5];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[6];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[7];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[8];
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                foreach (Renderer childRenderer in lastClickedObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = colours[9];
                }
            }
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

        return lastClickedObject; // Return null if no object is clicked
    }

    private IEnumerator RotateCamera(int phase)
    {
        float elapsedTime = 0.001f;
        float turnSpeed = 800f;
        float duration = 6.5f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(transform.position, phasePositions[phase].position, elapsedTime / turnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, phasePositions[phase].rotation, elapsedTime / turnSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public GameObject GetLastSelectedGameObject()
    {
        if (lastClickedObject != null)
        {
            return lastClickedObject;
        }
        else
        {
            return null;
        }
    }

    public void SwitchPhase()
    {
        source.PlayOneShot(switchPhase);
        if (isMaterialPhase)
        {
            isMaterialPhase = false;
            isDesignPhase = true;
            CanRotate = true;
        }
        else if (isDesignPhase)
        {
            isDesignPhase = false;
            isMaterialPhase = true;
            CanRotate = true;
        }
    }
}
