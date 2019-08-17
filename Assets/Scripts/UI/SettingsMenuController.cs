using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject generalPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject inputPanel;

    [Header("Panel Buttons")]
    [SerializeField] private Selectable generalButton;
    [SerializeField] private Selectable videoButton;
    [SerializeField] private Selectable soundButton;
    [SerializeField] private Selectable inputButton;

    [Header("General UI Widgets")]
    [SerializeField] private UISlider gameSpeedSlider;
    [SerializeField] private UIToggle autoAimToggle;
    [SerializeField] private UISlider autoAimStrengthSlider;

    [Header("Video UI Widgets")]
    [SerializeField] private UIToggle dyslexicTextToggle;
    [SerializeField] private UIDropdown resolutionDropdown;
    [SerializeField] private UIToggle fullscreenToggle;

    [Header("Input UI Widgets")]
    [SerializeField] private UISlider inputSensitivitySlider;
    [SerializeField] private UIToggle rumbleEnabledToggle;
    [SerializeField] private UISlider rumbleSensitivitySlider;
    [SerializeField] private Button rebindControlsButton;

    [Header("Bottom Buttons")]
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button applyChangesButton;

    private Navigation backNav;
    private Navigation applyNav;

    [Inject] private SettingsManager settingsManager;
    [Inject] private MenuController menuController;
    [Inject] private IInputController inputController;
    [Inject] private SceneController sceneController;

    void Start()
    {
        var gameSpeed = settingsManager.CurrentGameSpeed();
        var dyslexicText = settingsManager.IsDyslexicTextEnabled();
        var supportedResolutions = settingsManager.ResolutionsSupported();
        var fullscreenEnabled = settingsManager.IsFullscreenEnabled();
        var autoAimEnabled = settingsManager.IsAutoAimEnabled();
        var autoAimStrength = settingsManager.GetAutoAimStrength();
        var inputSensitivity = settingsManager.GetInputSensitivity();
        var rumbleEnabled = settingsManager.IsRumbleEnabled();
        var rumbleSensitivity = settingsManager.GetRumbleSensitivity();

        // General
        gameSpeedSlider.SetValue((int)gameSpeed);
        autoAimToggle.SetValue(autoAimEnabled);
        autoAimStrengthSlider.SetValue(autoAimStrength);

        // Video
        dyslexicTextToggle.SetValue(dyslexicText);
        resolutionDropdown.SetOptions(supportedResolutions);
        fullscreenToggle.SetValue(fullscreenEnabled);

        settingsManager.UpdateFont();

        // Sound

        // Input
        //inputSensitivitySlider.SetValue(inputSensitivity);
        //rumbleEnabledToggle.SetValue(rumbleEnabled);
        //rumbleSensitivitySlider.SetValue(rumbleSensitivity);

        backNav = backToMainMenuButton.navigation;
        applyNav = applyChangesButton.navigation;

        SwapToGeneral();
    }

    void OnEnable()
    {
        //backToMainMenuButton.Select();
        //backToMainMenuButton.OnSelect(null);
        generalButton.Select();
        generalButton.OnSelect(null);
    }

    // Update is called once per frame
    void Update()
    {
        settingsManager.UpdateFont();

        if (inputController.UICancel()) {
            // If settings have changed
            // Show dialog box asking to save changes
            // else 
            // If Main Menu
            // Go back to main menu
            // else
            // Go back to pause menu

            if (sceneController.IsInGame()) {

            } else {
                menuController.SwapToMainMenu();
            }
        }
    }

    public void ApplyChanges()
    {
        // Throw up a dialog box asking if you wish to save those changes

        settingsManager.SetResolution(resolutionDropdown.GetValue());
    }

    public void AutoAimToggle(bool value)
    {
        var toggle = autoAimToggle.GetObject();
        var slider = autoAimStrengthSlider.GetObject();

        Navigation nav = toggle.navigation;

        // Rewire the navigation first
        if (value) {
            autoAimStrengthSlider.gameObject.SetActive(true);
            nav.selectOnDown = slider;
            toggle.navigation = nav;
        } else {
            autoAimStrengthSlider.gameObject.SetActive(false);
            nav.selectOnDown = applyChangesButton;
            toggle.navigation = nav;
        }

        settingsManager.AutoAimToggle();
    }

    public void DyslexicToggle()
    {
        settingsManager.DyslexicToggle();
    }

    public void FullscreenToggle()
    {
        settingsManager.FullScreenToggle();
    }

    public void RumbleToggle(bool value)
    {
        var toggle = rumbleEnabledToggle.GetObject();
        var slider = rumbleSensitivitySlider.GetObject();

        Navigation nav = toggle.navigation;
        if (value) {
            rumbleSensitivitySlider.gameObject.SetActive(true);
            nav.selectOnDown = slider;
            toggle.navigation = nav;
        } else {
            rumbleSensitivitySlider.gameObject.SetActive(false);
            nav.selectOnDown = rebindControlsButton;
            toggle.navigation = nav;
        }


        settingsManager.RumbleToggle();
    }

    #region Panel Switch Methods

    public void SwapToGeneral()
    {
        // Set Active
        generalPanel.SetActive(true);
        videoPanel.SetActive(false);
        soundPanel.SetActive(false);
        inputPanel.SetActive(false);

        // Modify Navigation for back and apply buttons
        var autoAimEnabled = settingsManager.IsAutoAimEnabled();
        if (autoAimEnabled) {
            backNav.selectOnUp = autoAimStrengthSlider.GetObject();
            applyNav.selectOnUp = autoAimStrengthSlider.GetObject();
        } else {
            backNav.selectOnUp = autoAimToggle.GetObject();
            applyNav.selectOnUp = autoAimToggle.GetObject();
        }

        backToMainMenuButton.navigation = backNav;
        applyChangesButton.navigation = applyNav;
    }

    public void SwapToVideo()
    {
        // Set Active
        generalPanel.SetActive(false);
        videoPanel.SetActive(true);
        soundPanel.SetActive(false);
        inputPanel.SetActive(false);

        // Modify Navigation for back and apply buttons
        backNav.selectOnUp = fullscreenToggle.GetObject();
        applyNav.selectOnUp = fullscreenToggle.GetObject();

        backToMainMenuButton.navigation = backNav;
        applyChangesButton.navigation = applyNav;
    }

    public void SwapToSound()
    {
        // Set Active
        generalPanel.SetActive(false);
        videoPanel.SetActive(false);
        soundPanel.SetActive(true);
        inputPanel.SetActive(false);

        // Modify Navigation for back and apply buttons
        backNav.selectOnUp = soundButton;
        applyNav.selectOnUp = soundButton;

        backToMainMenuButton.navigation = backNav;
        applyChangesButton.navigation = applyNav;
    }

    public void SwapToInput()
    {
        // Set Active
        generalPanel.SetActive(false);
        videoPanel.SetActive(false);
        soundPanel.SetActive(false);
        inputPanel.SetActive(true);

        // Modify Navigation for back and apply buttons
        backNav.selectOnUp = rebindControlsButton;
        applyNav.selectOnUp = rebindControlsButton;

        backToMainMenuButton.navigation = backNav;
        applyChangesButton.navigation = applyNav;
    }

    #endregion

    #region Menu Swapping
    public void SwapToControlMapper()
    {
        menuController.SwapToControlMapper();
    }

    public void ReturnFromControlMapper()
    {
        Logger.Debug("Should be swapping back here");
        menuController.SwapToSettingsMenu();
        SwapToInput();

        inputButton.Select();
        inputButton.OnSelect(null);
    }

    public void BackToMainMenu()
    {
        menuController.SwapToMainMenu();
    }

    #endregion
}
