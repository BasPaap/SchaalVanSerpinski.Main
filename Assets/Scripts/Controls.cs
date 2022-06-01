using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static event EventHandler PentagramStartTriggered;
    public static event EventHandler PentagramEndTriggered;
    public static event EventHandler GongTriggered;
    public static event EventHandler<VideoTriggeredEventHandler> VideoTriggered;

    [SerializeField] private KeyCode pentagramStartKey = KeyCode.P;
    [SerializeField] private KeyCode pentagramEndKey = KeyCode.LeftBracket;
    [SerializeField] private KeyCode toggleGongKey = KeyCode.G;
    [SerializeField] private KeyCode videoKey = KeyCode.V;

    public void TriggerPentagramStart()
    {
        PentagramStartTriggered(this, EventArgs.Empty);
    }

    public void TriggerPentagramEnd()
    {
        PentagramEndTriggered(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (Input.GetKeyUp(pentagramStartKey) && PentagramStartTriggered != null)
        {
            TriggerPentagramStart();
        }

        if (Input.GetKeyUp(pentagramEndKey) && PentagramEndTriggered != null)
        {
            TriggerPentagramEnd();
        }

        if (Input.GetKeyUp(toggleGongKey) && GongTriggered != null)
        {
            GongTriggered(this, EventArgs.Empty);
        }

        if (Input.GetKeyUp(videoKey) && VideoTriggered != null)
        {
            VideoTriggered(this, new VideoTriggeredEventHandler());
        }
    }
}
