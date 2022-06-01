using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class VideoTriggeredEventHandler
{
    public string ClipName { get; set; }

    public VideoTriggeredEventHandler()
    {
    }

    public VideoTriggeredEventHandler(string clipName)
    {
        ClipName = clipName;
    }
}
