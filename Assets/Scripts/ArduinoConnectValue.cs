using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoConnectValue : MonoBehaviour
{
    #region Instancing
    public static ArduinoConnectValue Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ArduinoConnectValue>();
                if (_instance == null)
                {
                    _instance = new GameObject("ArduinoConnect").AddComponent<ArduinoConnectValue>();
                }
            }
            return _instance;
        }
    }
    private static ArduinoConnectValue _instance;
    public string value;
    public SerialPort stream = new SerialPort("COM3", 9600);
    private void Awake()
    {
        // Destroy any duplicate instances that may have been created
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
    }
    #endregion
    void Start()
    {
        stream.Open();
        stream.ReadTimeout = 5000;
    }
    void FixedUpdate()
    {
        if (stream.IsOpen)
        {
            try
            {
                Update();

            }
            catch (System.Exception)
            {

            }
        }

    }
    public void Update()
    {
        value = stream.ReadLine();
    }

    public void CloseCom()
    {
        stream.Close();
    }
}