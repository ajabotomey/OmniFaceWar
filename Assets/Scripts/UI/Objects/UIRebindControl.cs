using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIRebindControl : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private string bindingID;
    [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;

    [Header("Device Display Settings")]
    [SerializeField] private DeviceDisplayConfigurator deviceDisplaySettings;

    [Header("UI - Label")]
    [SerializeField] private TextMeshProUGUI actionLabel;

    [Header("UI - Input Button")]
    [SerializeField] private Button inputButton;
    [SerializeField] private TextMeshProUGUI inputButtonLabel;
    [SerializeField] private Image inputButtonImage;

    [Header("UI - Reset Button")]
    [SerializeField] private Button resetButton;

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    private InputAction action;

    public string BindingId
    {
        get => bindingID;
        set
        {
            bindingID = value;
            UpdateBindingDisplayUI();
        }
    }

    void Awake()
    {
        action = actionReference.action;
    }

    public void UpdateBehaviour()
    {
        if (action == null)
            action = actionReference.action;

        UpdateActionDisplayUI();
        UpdateBindingDisplayUI();
    }

    public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex)
    {
        bindingIndex = -1;

        action = actionReference?.action;
        if (action == null)
            return false;

        if (string.IsNullOrEmpty(bindingID))
            return false;

        // Look up binding index.
        var bindingId = new Guid(bindingID);
        bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
        if (bindingIndex == -1)
        {
            Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
            return false;
        }

        return true;
    }

    public void StartRebindProcess()
    {
        ToggleGameObjectState(resetButton.gameObject, false);

        if (!ResolveActionAndBinding(out var action, out var bindingIndex))
            return;

        PerformInteractiveRebind(action, bindingIndex);
    }

    private void PerformInteractiveRebind(InputAction action, int bindingIndex)
    {
        rebindOperation?.Cancel();

        void CleanUp()
        {
            rebindOperation?.Dispose();
            rebindOperation = null;
        }

        action.Disable();

        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnCancel(
                operation => {
                    action.Enable();
                    UpdateBehaviour();
                    CleanUp();
                }
            )
            .OnComplete(
                operation => {
                    action.Enable();
                    RebindCompleted();
                }
            );

        rebindOperation.Start();

    }

    void RebindCompleted()
    {
        rebindOperation.Dispose();
        rebindOperation = null;

        ToggleGameObjectState(resetButton.gameObject, true);

        UpdateActionDisplayUI();
        UpdateBindingDisplayUI();
    }

    public void ResetBinding()
    {
        InputActionRebindingExtensions.RemoveAllBindingOverrides(action);

        UpdateBehaviour();
    }

    void UpdateActionDisplayUI()
    {
        //actionLabel.SetText(action.name);
        // Get the Binding name
        var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == bindingID);
        if (bindingIndex != -1)
        {
            actionLabel.SetText(action.bindings[bindingIndex].name);
        }
    }

    void UpdateBindingDisplayUI()
    {
        // Get the sprite if it exists
        int controlBindingIndex = action.GetBindingIndexForControl(action.controls[0]);
        string currentBindingInput = InputControlPath.ToHumanReadableString(action.bindings[controlBindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        Sprite currentDisplayIcon = deviceDisplaySettings.GetDeviceBindingIcon(playerInput, currentBindingInput);

        if(currentDisplayIcon)
        {
            ToggleGameObjectState(inputButtonLabel.gameObject, false);
            ToggleGameObjectState(inputButtonImage.gameObject, true);
            inputButtonImage.sprite = currentDisplayIcon;
        } else if(currentDisplayIcon == null)
        {
            var displayString = GetBindingString(action);

            ToggleGameObjectState(inputButtonLabel.gameObject, true);
            ToggleGameObjectState(inputButtonImage.gameObject, false);
            inputButtonLabel.SetText(displayString);
        }
    }

    void ToggleGameObjectState(GameObject targetGameObject, bool newState)
    {
        targetGameObject.SetActive(newState);
    }

    private string GetBindingString(InputAction action)
    {
        var displayString = string.Empty;
        var deviceLayoutName = default(string);
        var controlPath = default(string);

        var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == bindingID);
        if (bindingIndex != -1)
        {
            displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, DisplayStringOptions);
        }

        return displayString;
    }

    public InputBinding.DisplayStringOptions DisplayStringOptions
    {
        get => displayStringOptions;
        set
        {
            displayStringOptions = value;
            UpdateBindingDisplayUI();
        }
    }
}
