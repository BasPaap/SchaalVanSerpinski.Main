using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pentagram : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
        audioSource.Play();
    }
}
