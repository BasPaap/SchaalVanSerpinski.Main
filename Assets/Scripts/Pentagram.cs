using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagram : MonoBehaviour
{
    private void OnEnable()
    {
        Controls.PentagramTriggered += Controls_PentagramTriggered;
    }

    private void OnDisable()
    {
        Controls.PentagramTriggered -= Controls_PentagramTriggered;
    }

    private void Controls_PentagramTriggered(object sender, System.EventArgs e)
    {
        Debug.Log("Sending pentagram command to hardware host");
        HardwareHost.SendCommand(HardwareCommand.Pentagram);
    }
}
