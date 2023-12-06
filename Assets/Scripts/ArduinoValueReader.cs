using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoValueReader : MonoBehaviour
{
    public string portName = "COM3";
    public int baudRate = 9600;
    private SerialPort sp;
    private string currentMessage;
    private string lastMessage; // Added field to store the last received message

    void Start()
    {
        sp = new SerialPort(portName, baudRate);
        sp.Open();
        sp.ReadTimeout = 10;
    }

    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                currentMessage = sp.ReadLine().Trim();

                // Check if the current message is different from the last one
                if (!string.IsNullOrEmpty(currentMessage) && currentMessage != lastMessage)
                {
                    //Debug.Log("Received message: " + currentMessage);
                    lastMessage = currentMessage; // Update the last received message
                }
            }
            catch (System.Exception)
            {
                // Handle exceptions, if any.
            }
        }
    }

    // Expose the current message through a method
    public string GetCurrentMessage()
    {
        return currentMessage;
    }

    void OnDestroy()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }
}
