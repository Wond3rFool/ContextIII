using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoValueReader : MonoBehaviour
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
    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort sp;

    void Start()
    {
        sp = new SerialPort(portName, baudRate);
        sp.Open();
        sp.ReadTimeout = 1;
    }

    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                string message = sp.ReadLine().Trim(); // Trim removes leading/trailing whitespaces

                if (!string.IsNullOrEmpty(message))
                {
                    int buttonNumber;
                    if (int.TryParse(message, out buttonNumber))
                    {
                        // Handle button presses with use cases
                        switch (buttonNumber)
                        {
                            case 1:
                                HandleButton1();
                                break;
                            case 2:
                                HandleButton2();
                                break;
                            case 3:
                                HandleButton3();
                                break;
                            case 4:
                                HandleButton4();
                                break;
                            case 5:
                                HandleButton5();
                                break;

                        }
                    }
                }
            }
            catch (System.Exception)
            {
                // Handle exceptions, if any.
            }
        }
    }

    void HandleButton1()
    {
        // Add your use case for Button 1 here
        Debug.Log("Button 1 Pressed");
        Instantiate(prefabToInstantiate1, new Vector3(0, 0, 15), Quaternion.identity);
    }

    void HandleButton2()
    {
        // Add your use case for Button 2 here
        Debug.Log("Button 2 Pressed");
        Instantiate(prefabToInstantiate2, new Vector3(0, 0, 15), Quaternion.identity);
    }

    void HandleButton3()
    {
        // Add your use case for Button 3 here
        Debug.Log("Button 3 Pressed");
        Instantiate(prefabToInstantiate3, new Vector3(0, 0, 15), Quaternion.identity);
    }

    void HandleButton4()
    {
        // Add your use case for Button 4 here
        Debug.Log("Button 4 Pressed");
        Instantiate(prefabToInstantiate4, new Vector3(0, 0, 15), Quaternion.identity);
    }

    void HandleButton5()
    {
        // Add your use case for Button 5 here
        Debug.Log("Button 5 Pressed");
        Instantiate(prefabToInstantiate5, new Vector3(0, 0, 15), Quaternion.identity);
    }

    void OnDestroy()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }
}

