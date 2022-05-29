using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static event EventHandler PentagramTriggered;

    [SerializeField] private KeyCode pentagramKey = KeyCode.P;

    private void Update()
    {
        if (Input.GetKeyUp(pentagramKey) && PentagramTriggered != null)
        {
            PentagramTriggered(this, EventArgs.Empty);
        }
    }
}
