using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

public class HardwareHost : MonoBehaviour
{
    public string handshake;
    public string expectedResponse;

    private static SerialPort serialPort;
    private static readonly Dictionary<HardwareCommand, string> serialCommands = new Dictionary<HardwareCommand, string>();
    private static bool isConnected = false;

    private void Start()
    {
        serialCommands.Add(HardwareCommand.Pentagram, "PENTAGRAM");

        var settings = Settings.Load();

        if (SerialPort.GetPortNames().Contains(settings.PortName))
        {
            Debug.Log($"Attempting to open port {settings.PortName} @ {settings.BaudRate}.");
            serialPort = new SerialPort(settings.PortName, settings.BaudRate);

            try
            {
                serialPort.Open();
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.Log($"UnauthorizedAccessException on port {settings.PortName}: {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.Log($"ArgumentOutOfRangeException on port {settings.PortName}, baud rate {settings.BaudRate}: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Debug.Log($"ArgumentException on port {settings.PortName}, baud rate {settings.BaudRate}: {ex.Message}");
            }
            catch (IOException ex)
            {
                Debug.Log($"IOException on port {settings.PortName}, baud rate {settings.BaudRate}: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Debug.Log($"InvalidOperationException on port {settings.PortName}, baud rate {settings.BaudRate}: {ex.Message}");
            }

            Debug.Log("Serial port open");
        }
        else
        {
            Debug.Log($"Port {settings.PortName} was not found.");
        }
    }

    private void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            if (serialPort.BytesToRead > 0)
            {
                var lastLineRead = serialPort.ReadLine();
                Debug.Log($"Received response on serial port {serialPort.PortName}: {lastLineRead}");

                if (!isConnected && lastLineRead == expectedResponse)
                {
                    Debug.Log("Exchanged handshake and response, now connected to device.");
                    isConnected = true;
                }
            }

            if (!isConnected)
            {
                serialPort.Write($"{handshake}\n");
            }
        }
    }


    public static void SendCommand(HardwareCommand command)
    {
        var commandToSend = serialCommands[command];

        if (isConnected)
        {
            Debug.Log($"Writing command {commandToSend} to port {serialPort.PortName}.");
            serialPort.Write($"{commandToSend}\n");
        }
        else
        {
            Debug.Log($"SendCommand called for {commandToSend} command but the device is not yet connected.");
        }
    }

    private void OnDestroy()
    {
        serialPort?.Close();
    }
}
