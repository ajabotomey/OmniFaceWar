using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebindController : MonoBehaviour
{
    private InputActionAsset actionAsset;
    private PlayerInput playerInput;
    private InputAction inputAction;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    // Save any overrides to eventually save to save file
    public static Dictionary<string, string> OverridesDictionary = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
