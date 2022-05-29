using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static event EventHandler PentagramTriggered;
    public static event EventHandler GongTriggered;

    [SerializeField] private KeyCode pentagramKey = KeyCode.P;
    [SerializeField] private KeyCode toggleGongKey = KeyCode.G;

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
    }
}
