using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static event EventHandler PentagramStartTriggered;
    public static event EventHandler PentagramEndTriggered;
    public static event EventHandler GongTriggered;

    [SerializeField] private KeyCode pentagramStartKey = KeyCode.P;
    [SerializeField] private KeyCode pentagramEndKey = KeyCode.LeftBracket;
    [SerializeField] private KeyCode toggleGongKey = KeyCode.G;

    private void Update()
    {
        if (Input.GetKeyUp(pentagramStartKey) && PentagramStartTriggered != null)
        {
            PentagramStartTriggered(this, EventArgs.Empty);
        }

        if (Input.GetKeyUp(pentagramEndKey) && PentagramEndTriggered != null)
        {
            PentagramEndTriggered(this, EventArgs.Empty);
        }

        if (Input.GetKeyUp(toggleGongKey) && GongTriggered != null)
        {
            GongTriggered(this, EventArgs.Empty);
        }
    }
}
