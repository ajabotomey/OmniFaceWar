using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SettingsManager {

    [Header("Text")]
    [SerializeField] private Font classicFont;
    [SerializeField] private Font dyslexicFont;
    [SerializeField] private bool dyslexicTextEnabled;
    [SerializeField] [Range(28, 100)] private int textSize = 28;
    private int TEXT_SIZE_MIN = 28;
    private int TEXT_SIZE_MAX = 100;

    [Header("Game Speed")]
    [SerializeField] [Range(10, 100)] private float gameSpeed = 100;
    private int GAME_SPEED_MIN = 10; // 10% speed
    private int GAME_SPEED_MAX = 100; // 100% Speed

    [Header("Subtitles")]
    [SerializeField] private bool subtitlesEnabled = false;
    [SerializeField] [Range(42, 100)] private int subtitleTextSize = 42;
    private int SUBTITLE_MIN_SIZE = 42;
    private int SUBTITLE_MAX_SIZE = 100;
    [SerializeField] private bool subtitleBackgroundEnabled = true;
    [SerializeField] private bool subtitleColorEnabled = true;

    [Header("Auto-aim")]
    [SerializeField] private bool autoAimEnabled = false;
    [SerializeField] [Range(1, 100)] private int autoAimStrength = 100;
    private int AUTOAIM_STRENGTH_MIN = 1;
    private int AUTOAIM_STRENGTH_MAX = 100;

    [Header("Fullscreen")]
    [SerializeField] private bool fullscreenEnabled = false;
    private Resolution[] resolutions;
    private int currentResolutionIndex;
    private List<string> resolutionsSupported;
    private int screenWidth, screenHeight;

    [Header("Input Sensitivity")]
    [SerializeField] [Range(1, 10)] private int inputSensitivity = 5;
    private int INPUT_SENSITIVITY_MIN = 1;
    private int INPUT_SENSITIVITY_MAX = 10;
    [SerializeField] private bool rumbleEnabled = true;
    [SerializeField] [Range(1, 100)] private int rumbleSensitivity = 100;
    private int RUMBLE_SENSITIVITY_MIN = 1;
    private int RUMBLE_SENSITIVITY_MAX = 100;

    #region Accessor Methods

    public bool IsDyslexicTextEnabled()
    {
        return dyslexicTextEnabled;
    }

    public bool IsFullscreenEnabled()
    {
        return fullscreenEnabled;
    }

    public float CurrentGameSpeed()
    {
        return gameSpeed;
    }

    public List<string> ResolutionsSupported()
    {
        return resolutionsSupported;
    }

    public bool IsAutoAimEnabled()
    {
        return autoAimEnabled;
    }

    public int GetAutoAimStrength()
    {
        return autoAimStrength;
    }

    public bool IsSubtitlesEnabled()
    {
        return subtitlesEnabled;
    }

    public bool IsSubtitleBackgroundEnabled()
    {
        return subtitleBackgroundEnabled;
    }

    public int GetSubtitleTextSize()
    {
        return subtitleTextSize;
    }

    public int GetTextSize()
    {
        return textSize;
    }

    public int GetInputSensitivity()
    {
        return inputSensitivity;
    }

    public bool IsRumbleEnabled()
    {
        return rumbleEnabled;
    }

    public int GetRumbleSensitivity()
    {
        return rumbleSensitivity;
    }

    public int GetWindowWidth()
    {
        return screenWidth;
    }

    public int GetWindowHeight()
    {
        return screenHeight;
    }

    #endregion

    #region Mutator Methods

    public void GameSpeedToggle(int value)
    {
        if (value >= GAME_SPEED_MIN && value <= GAME_SPEED_MAX)
            gameSpeed = value;
    }

    public void DyslexicToggle()
    {
        dyslexicTextEnabled = !dyslexicTextEnabled;
    }

    public void SetTextSize(int value)
    {
        if (value >= TEXT_SIZE_MIN && value <= TEXT_SIZE_MAX)
            textSize = value;
    }

    public void SetResolution(string value)
    {
        var resolutionString = value;
        string[] values = resolutionString.Split(new string[] { " x " }, StringSplitOptions.RemoveEmptyEntries);

        screenWidth = Int32.Parse(values[0]);
        screenHeight = Int32.Parse(values[1]);

        Screen.SetResolution(screenWidth, screenHeight, fullscreenEnabled);
    }

    public void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, fullscreenEnabled);
    }

    public void FullScreenToggle()
    {
        fullscreenEnabled = !fullscreenEnabled;
    }

    public void AutoAimToggle()
    {
        autoAimEnabled = !autoAimEnabled;
    }

    public void SetAutoAimStrength(int value)
    {
        if (value >= AUTOAIM_STRENGTH_MIN && value <= AUTOAIM_STRENGTH_MAX)
            autoAimStrength = value;
    }

    public void SubtitlesToggle()
    {
        subtitlesEnabled = !subtitlesEnabled;
    }

    public void SubtitleBackgroundToggle()
    {
        subtitleBackgroundEnabled = !subtitleBackgroundEnabled;
    }

    public void SetSubtitleText(int value)
    {
        if (value >= SUBTITLE_MIN_SIZE && value <= SUBTITLE_MAX_SIZE)
            subtitleTextSize = value;
    }

    public void SetInputSensitivity(int value)
    {
        if (value >= INPUT_SENSITIVITY_MIN && value <= INPUT_SENSITIVITY_MAX)
            inputSensitivity = value;
    }

    public void RumbleToggle()
    {
        rumbleEnabled = !rumbleEnabled;
    }

    public void SetRumbleSensitivity(int value)
    {
        if (value >= RUMBLE_SENSITIVITY_MIN && value <= RUMBLE_SENSITIVITY_MAX)
            rumbleSensitivity = value;
    }

    #endregion

    public void UpdateFont()
    {
        var textComponents = Component.FindObjectsOfType<Text>();

        if (dyslexicTextEnabled) { // Change all text to use Dyslexic font
            foreach (var component in textComponents)
                component.font = dyslexicFont;
        } else { // Change back to Arial
            foreach (var component in textComponents) {
                if (component.gameObject.name != "DyslexicText")
                    component.font = classicFont;
            }
        }
    }
}
