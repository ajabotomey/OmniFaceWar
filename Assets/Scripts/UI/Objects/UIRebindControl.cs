using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIRebindControl : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionReference actionReference;

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

    public void UpdateBehaviour()
    {
        UpdateActionDisplayUI();
        UpdateBindingDisplayUI();
    }

    void StartRebindProcess()
    {
        ToggleGameObjectState(resetButton.gameObject, false);

        rebindOperation = actionReference.action.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindCompleted());
    }

    void RebindCompleted()
    {
        rebindOperation.Dispose();
        rebindOperation = null;

        ToggleGameObjectState(resetButton.gameObject, true);
    }

    void ResetBinding()
    {
        InputActionRebindingExtensions.RemoveAllBindingOverrides(actionReference.action);
    }

    void UpdateActionDisplayUI()
    {
        actionLabel.SetText(action.name);
    }

    void UpdateBindingDisplayUI()
    {
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
            ToggleGameObjectState(inputButtonLabel.gameObject, true);
            ToggleGameObjectState(inputButtonImage.gameObject, false);
            inputButtonLabel.SetText(currentBindingInput);
        }
    }

    void ToggleGameObjectState(GameObject targetGameObject, bool newState)
    {
        targetGameObject.SetActive(newState);
    }
}
