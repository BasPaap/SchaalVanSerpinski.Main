using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pentagram : MonoBehaviour
{
    [SerializeField] private AudioClip pentagramStartClip;
    [SerializeField] private AudioClip pentagramEndClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        Controls.PentagramStartTriggered += Controls_PentagramStartTriggered;
        Controls.PentagramEndTriggered += Controls_PentagramEndTriggered;
    }

    private void OnDisable()
    {
        Controls.PentagramStartTriggered -= Controls_PentagramStartTriggered;
        Controls.PentagramEndTriggered -= Controls_PentagramEndTriggered;
    }

    private void Controls_PentagramStartTriggered(object sender, System.EventArgs e)
    {
        Debug.Log("Sending pentagram start command to hardware host");
        HardwareHost.SendCommand(HardwareCommand.PentagramStart);
        audioSource.PlayOneShot(pentagramStartClip);
    }

    private void Controls_PentagramEndTriggered(object sender, System.EventArgs e)
    {
        Debug.Log("Sending pentagram end command to hardware host");
        HardwareHost.SendCommand(HardwareCommand.PentagramEnd);
        audioSource.PlayOneShot(pentagramEndClip);
    }    
}
