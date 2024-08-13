using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoController : MonoBehaviour
{
    public static ArduinoController Instance { get; private set; }

    public string portName = "COM3";
    public int baudRate = 9600;
    private SerialPort serialPort;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        try
        {
            serialPort.Open();
            Debug.Log("Serial Port Opened");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    public void TriggerVibration()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine("1");
                Debug.Log("Vibration Signal Sent to Arduino");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to send data to Arduino: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Serial port is not open");
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial Port Closed");
        }
    }
}
