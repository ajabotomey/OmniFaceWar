using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebindController : MonoBehaviour
{
    [Header("UI - Buttons")]
    [SerializeField] private Button applyChangesButton;
    [SerializeField] private Button resetChangesButton;
    [SerializeField] private Button backButton;

    [Header("UI - Rebind Controls")]
    [SerializeField] private UIRebindControl[] rebindControls;

    void Awake()
    {
        foreach (UIRebindControl control in rebindControls)
        {
            control.UpdateBehaviour();
        }
    }

    public void ApplyChanges()
    {
        // Save the changes into a dictionary to put into the save file later on
    }

    public void ResetChanges()
    {
        // Reset all of the changes that have been made to the default control configuration
    }
}
