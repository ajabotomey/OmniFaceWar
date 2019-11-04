using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;

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
    [SerializeField] private UISlider textSizeSlider;
    [SerializeField] private UIDropdown resolutionDropdown;
    [SerializeField] private UIToggle fullscreenToggle;
    [SerializeField] private UIToggle subtitlesEnabledToggle;
    [SerializeField] private UISlider subtitleTextSizeSlider;
    [SerializeField] private UISlider subtitleOpacitySlider;

    [Header("Sound UI Widgets")]
    [SerializeField] private UISlider soundFXVolumeSlider;
    [SerializeField] private UISlider musicVolumeSlider;
    [SerializeField] private UISlider voiceVolumeSlider;
    [SerializeField] private UIDropdown audioPlaybackTypeDropdown;

    [Header("Input UI Widgets")]
    [SerializeField] private UISlider inputSensitivitySlider;
    [SerializeField] private UIToggle rumbleEnabledToggle;
    [SerializeField] private UISlider rumbleSensitivitySlider;
    [SerializeField] private Button rebindControlsButton;

    [Header("Bottom Buttons")]
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button applyChangesButton;

    [Header("Game Events")]
    [SerializeField] private VoidEvent originalMenuEvent;
    [SerializeField] private VoidEvent controlMapperEvent;
    [SerializeField] private VoidEvent settingsMenuEvent;

    private Navigation backNav;
    private Navigation applyNav;
    private Navigation generalNav;
    private Navigation videoNav;
    private Navigation soundNav;
    private Navigation inputNav;

    [Inject] private SettingsManager settingsManager;
    [Inject] private IInputController inputController;
    [Inject] private SceneController sceneController;

    void Start()
    {
        // General
        var gameSpeed = settingsManager.CurrentGameSpeed();
        var autoAimEnabled = settingsManager.IsAutoAimEnabled();
        var autoAimStrength = settingsManager.GetAutoAimStrength();
        gameSpeedSlider.SetValue((int)gameSpeed);
        autoAimToggle.SetValue(autoAimEnabled);
        autoAimStrengthSlider.SetValue(autoAimStrength);

        // Video
        var dyslexicText = settingsManager.IsDyslexicTextEnabled();
        var textSize = settingsManager.CurrentTextSize();
        var supportedResolutions = settingsManager.ResolutionsSupported();
        var fullscreenEnabled = settingsManager.IsFullscreenEnabled();
        var subtitlesEnabled = settingsManager.IsSubtitlesEnabled();
        var subtitleTextSize = settingsManager.GetSubtitleTextSize();
        var subtitleOpacity = settingsManager.GetSubtitleOpacity();
        dyslexicTextToggle.SetValueWithoutNotify(dyslexicText);
        textSizeSlider.SetValue(textSize);
        resolutionDropdown.SetOptions(supportedResolutions);
        fullscreenToggle.SetValue(fullscreenEnabled);
        subtitlesEnabledToggle.SetValue(subtitlesEnabled);
        subtitleTextSizeSlider.SetValue(subtitleTextSize);
        subtitleOpacitySlider.SetValue(subtitleOpacity);

        // Sound
        var soundFXVolume = settingsManager.GetSoundFXVolume();
        var musicVolume = settingsManager.GetMusicVolume();
        var voiceVolume = settingsManager.GetVoiceVolume();
        var audioPlaybackType = settingsManager.GetAudioPlaybackType();
        soundFXVolumeSlider.SetValue(soundFXVolume);
        musicVolumeSlider.SetValue(musicVolume);
        voiceVolumeSlider.SetValue(voiceVolume);
        PopulateAudioPlaybackDropdown();
        audioPlaybackTypeDropdown.SetValue(audioPlaybackType);

        // Input
        var inputSensitivity = settingsManager.GetInputSensitivity();
        var rumbleEnabled = settingsManager.IsRumbleEnabled();
        var rumbleSensitivity = settingsManager.GetRumbleSensitivity();
        inputSensitivitySlider.SetValue(inputSensitivity);
        rumbleEnabledToggle.SetValue(rumbleEnabled);
        rumbleSensitivitySlider.SetValue(rumbleSensitivity);

        // Setup navigation for menu buttons
        backNav = backToMainMenuButton.navigation;
        applyNav = applyChangesButton.navigation;
        generalNav = generalButton.navigation;
        videoNav = videoButton.navigation;
        soundNav = soundButton.navigation;
        inputNav = inputButton.navigation;

        settingsManager.LoadSettings();

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
                originalMenuEvent.Raise();
            }
        }
    }

    public void ConfirmChanges()
    {
        UIDialogBox.Instance.ShowPopUp("Would you like to save those changes?", ApplyChanges, HideDialogBox);
    }

    public void ApplyChanges()
    {
        settingsManager.SaveSettings();

        settingsManager.SetResolution(resolutionDropdown.GetValue());

        HideDialogBox();
    }

    public void HideDialogBox()
    {
        UIDialogBox.Instance.HidePopUp();

        applyChangesButton.Select();
        applyChangesButton.OnSelect(null);
    }

    public void PopulateAudioPlaybackDropdown()
    {
        string[] enumNames = Enum.GetNames(typeof(AudioSpeakerMode));
        List<string> names = new List<string>(enumNames);
        audioPlaybackTypeDropdown.SetOptions(names);
    }

    #region Change Value methods
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
        settingsManager.UpdateFont();
    }

    public void FullscreenToggle()
    {
        settingsManager.FullScreenToggle();
    }

    public void SubtitleToggle(bool value)
    {
        // Get Subtitle controls
        var subtitleToggle = subtitlesEnabledToggle.GetObject();
        var subTextSizeSlider = subtitleTextSizeSlider.GetObject();
        var backgroundOpacitySlider = subtitleOpacitySlider.GetObject();

        // Get other affected controls
        var textSizeObj = textSizeSlider.GetObject();
        var dropdownObj = resolutionDropdown.GetObject();

        // Get the navigation objects to modify
        Navigation toggleNav = subtitleToggle.navigation;
        Navigation textSizeNav = textSizeObj.navigation;
        Navigation dropdownNav = dropdownObj.navigation;

        // Now to modify those objects
        if (value) {
            subtitleTextSizeSlider.gameObject.SetActive(true);
            subtitleOpacitySlider.gameObject.SetActive(true);

            toggleNav.selectOnDown = subTextSizeSlider;
            textSizeNav.selectOnRight = subTextSizeSlider;
            dropdownNav.selectOnRight = backgroundOpacitySlider;
        } else {
            subTextSizeSlider.gameObject.SetActive(false);
            backgroundOpacitySlider.gameObject.SetActive(false);

            toggleNav.selectOnDown = applyChangesButton;
            textSizeNav.selectOnRight = null;
            dropdownNav.selectOnRight = null;
        }

        subtitleToggle.navigation = toggleNav;
        textSizeObj.navigation = textSizeNav;
        dropdownObj.navigation = dropdownNav;

        settingsManager.SubtitlesToggle();
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

    public void SetGameSpeed(float value)
    {
        settingsManager.SetGameSpeed((int)value);
    }

    public void SetAutoAimStrength(float value)
    {
        settingsManager.SetAutoAimStrength((int)value);
    }

    public void SetTextSize(float value)
    {
        settingsManager.SetTextSize((int)value);
    }

    public void SetSubtitleTextSize(float value)
    {
        settingsManager.SetSubtitleTextSize((int)value);
    }

    public void SetSubtitleBackgroundOpacity(float value)
    {
        settingsManager.SetSubtitleBackgroundOpacity((int)value);
    }

    public void SetSoundFXVolume(float value)
    {
        settingsManager.SetSoundFXVolume((int)value);
    }

    public void SetMusicVolume(float value)
    {
        settingsManager.SetMusicVolume((int)value);
    }

    public void SetVoiceVolume(float value)
    {
        settingsManager.SetVoiceVolume((int)value);
    }

    public void SetAudioPlaybackType(int index)
    {
        audioPlaybackTypeDropdown.SetValue(index);
        settingsManager.SetAudioPlaybackType(index);
    }

    public void SetInputSensitivity(float value)
    {
        settingsManager.SetInputSensitivity((int)value);
    }

    public void SetRumbleSensitivity(float value)
    {
        settingsManager.SetRumbleSensitivity((int)value);
    }

    #endregion

    #region Panel Switch Methods

    public void SwapToGeneral()
    {
        // Set Active
        generalPanel.SetActive(true);
        videoPanel.SetActive(false);
        soundPanel.SetActive(false);
        inputPanel.SetActive(false);

        // Modify navigation for menu buttons
        generalNav.selectOnDown = gameSpeedSlider.GetObject();
        videoNav.selectOnDown = gameSpeedSlider.GetObject();
        soundNav.selectOnDown = gameSpeedSlider.GetObject();
        inputNav.selectOnDown = gameSpeedSlider.GetObject();

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
        generalButton.navigation = generalNav;
        videoButton.navigation = videoNav;
        soundButton.navigation = soundNav;
        inputButton.navigation = inputNav;
    }

    public void SwapToVideo()
    {
        // Set Active
        generalPanel.SetActive(false);
        videoPanel.SetActive(true);
        soundPanel.SetActive(false);
        inputPanel.SetActive(false);

        // Modify navigation for menu buttons
        generalNav.selectOnDown = dyslexicTextToggle.GetObject();
        videoNav.selectOnDown = dyslexicTextToggle.GetObject();
        soundNav.selectOnDown = dyslexicTextToggle.GetObject();
        inputNav.selectOnDown = dyslexicTextToggle.GetObject();

        // Modify Navigation for back and apply buttons
        backNav.selectOnUp = fullscreenToggle.GetObject();
        applyNav.selectOnUp = fullscreenToggle.GetObject();

        backToMainMenuButton.navigation = backNav;
        applyChangesButton.navigation = applyNav;
        generalButton.navigation = generalNav;
        videoButton.navigation = videoNav;
        soundButton.navigation = soundNav;
        inputButton.navigation = inputNav;
    }

    public void SwapToSound()
    {
        // Set Active
        generalPanel.SetActive(false);
        videoPanel.SetActive(false);
        soundPanel.SetActive(true);
        inputPanel.SetActive(false);

        // Modify navigation for menu buttons

        // Modify Navigation for back and apply buttons
        backNav.selectOnUp = soundButton;
        applyNav.selectOnUp = soundButton;

        backToMainMenuButton.navigation = backNav;
        applyChangesButton.navigation = applyNav;
        generalButton.navigation = generalNav;
        videoButton.navigation = videoNav;
        soundButton.navigation = soundNav;
        inputButton.navigation = inputNav;
    }

    public void SwapToInput()
    {
        // Set Active
        generalPanel.SetActive(false);
        videoPanel.SetActive(false);
        soundPanel.SetActive(false);
        inputPanel.SetActive(true);

        // Modify navigation for menu buttons
        generalNav.selectOnDown = inputSensitivitySlider.GetObject();
        videoNav.selectOnDown = inputSensitivitySlider.GetObject();
        soundNav.selectOnDown = inputSensitivitySlider.GetObject();
        inputNav.selectOnDown = inputSensitivitySlider.GetObject();

        // Modify Navigation for back and apply buttons
        backNav.selectOnUp = rebindControlsButton;
        applyNav.selectOnUp = rebindControlsButton;

        backToMainMenuButton.navigation = backNav;
        applyChangesButton.navigation = applyNav;
        generalButton.navigation = generalNav;
        videoButton.navigation = videoNav;
        soundButton.navigation = soundNav;
        inputButton.navigation = inputNav;
    }

    #endregion

    #region Menu Swapping
    public void SwapToControlMapper()
    {
        //menuController.SwapToControlMapper();
        controlMapperEvent.Raise();
    }

    public void ReturnFromControlMapper()
    {
        Logger.Debug("Should be swapping back here");
        settingsMenuEvent.Raise();
        SwapToInput();

        inputButton.Select();
        inputButton.OnSelect(null);
    }

    public void BackToMainMenu()
    {
        originalMenuEvent.Raise();
    }

    #endregion
}
