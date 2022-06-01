using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static event EventHandler PentagramTriggered;
    public static event EventHandler GongTriggered;
    public static event EventHandler<VideoTriggeredEventHandler> VideoTriggered;

    [SerializeField] private KeyCode pentagramKey = KeyCode.P;
    [SerializeField] private KeyCode toggleGongKey = KeyCode.G;
    [SerializeField] private KeyCode videoKey = KeyCode.V;

    private void Update()
    {
        if (Input.GetKeyUp(pentagramKey) && PentagramTriggered != null)
        {
            PentagramTriggered(this, EventArgs.Empty);
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
