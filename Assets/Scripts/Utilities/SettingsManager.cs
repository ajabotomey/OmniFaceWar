using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class SettingsManager {

    [Header("Text")]
    [SerializeField] private Font classicFont;
    [SerializeField] private Font dyslexicFont;
    [SerializeField] private TMPro.TMP_FontAsset classicTMPFont;
    [SerializeField] private TMPro.TMP_FontAsset dyslexicTMPFont;
    [SerializeField] private bool dyslexicTextEnabled = false;
    [SerializeField] [Range(28, 100)] private int textSize = 28;
    private int TEXT_SIZE_MIN = 28;
    private int TEXT_SIZE_MAX = 42;

    [Header("Game Speed")]
    [SerializeField] [Range(10, 100)] private float gameSpeed = 100;
    private int GAME_SPEED_MIN = 10; // 10% speed
    private int GAME_SPEED_MAX = 100; // 100% Speed

    [Header("Subtitles")]
    [SerializeField] private bool subtitlesEnabled = false;
    [SerializeField] [Range(42, 100)] private int subtitleTextSize = 42;
    private int SUBTITLE_MIN_SIZE = 42;
    private int SUBTITLE_MAX_SIZE = 100;
    [SerializeField] [Range(0, 10)] private int subtitleBackgroundOpacity = 10;
    //[SerializeField] private bool subtitleBackgroundEnabled = true;

    [Header("Auto-aim")]
    [SerializeField] private bool autoAimEnabled = false;
    [SerializeField] [Range(1, 100)] private int autoAimStrength = 100;
    private int AUTOAIM_STRENGTH_MIN = 1;
    private int AUTOAIM_STRENGTH_MAX = 100;

    [Header("Fullscreen")]
    [SerializeField] private bool fullscreenEnabled = false;
    [SerializeField] private string currentResolution = "800x600";
    private Resolution[] resolutions;
    private int currentResolutionIndex;
    private List<string> resolutionsSupported;
    private int screenWidth, screenHeight;

    [Header("Input Settings")]
    [SerializeField] [Range(1, 10)] private int inputSensitivity = 5;
    private int INPUT_SENSITIVITY_MIN = 1;
    private int INPUT_SENSITIVITY_MAX = 10;
    [SerializeField] private bool rumbleEnabled = true;
    [SerializeField] [Range(1, 100)] private int rumbleSensitivity = 100;
    private int RUMBLE_SENSITIVITY_MIN = 1;
    private int RUMBLE_SENSITIVITY_MAX = 100;
    [SerializeField] private bool inputDelayEnabled = false;

    [Header("Audio Settings")]
    [SerializeField] [Range(0, 10)] private int soundFXVolume = 10;
    [SerializeField] [Range(0, 10)] private int musicVolume = 10;
    [SerializeField] [Range(0, 10)] private int voiceVolume = 10;
    private int SOUND_MIN = 0;
    private int SOUND_MAX = 10;
    [SerializeField] private AudioSpeakerMode audioSpeakerMode = AudioSpeakerMode.Stereo;

    public ModifiedSettings settings;

    [Inject] private IInputController _input;

    #region Accessor Methods

    public bool IsDyslexicTextEnabled()
    {
        return dyslexicTextEnabled;
    }

    public int CurrentTextSize()
    {
        return textSize;
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
        if (resolutionsSupported.Count == 0) {
            resolutions = Screen.resolutions;
            resolutionsSupported = new List<string>();

            for (int i = 3; i < resolutions.Length; i++) {

                var s = resolutions[i].ToString();
                var delimiter = s.IndexOf(" @");

                resolutionsSupported.Add(s.Substring(0, delimiter));
            }
        }

        return resolutionsSupported;
    }

    public string GetCurrentResolution()
    {
        return currentResolution;
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

    public int GetSubtitleTextSize()
    {
        return subtitleTextSize;
    }

    public int GetSubtitleOpacity()
    {
        return subtitleBackgroundOpacity;
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

    public int GetSoundFXVolume()
    {
        return soundFXVolume;
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }

    public int GetVoiceVolume()
    {
        return voiceVolume;
    }

    public int GetAudioPlaybackType()
    {
        return (int)audioSpeakerMode;
    }

    #endregion

    #region Mutator Methods

    public void SetGameSpeed(int value)
    {
        if (value >= GAME_SPEED_MIN && value <= GAME_SPEED_MAX)
            settings.gameSpeed = value;
    }

    public void DyslexicToggle()
    {
        dyslexicTextEnabled = !dyslexicTextEnabled;
    }

    public void SetTextSize(int value)
    {
        if (value >= TEXT_SIZE_MIN && value <= TEXT_SIZE_MAX)
            settings.textSize = value;
    }

    public void SetResolution(string value)
    {
        settings.currentResolution = value;
    }

    public void FullScreenToggle()
    {
        settings.fullscreenEnabled = !settings.fullscreenEnabled;
    }

    public void AutoAimToggle()
    {
        settings.autoAimEnabled = !settings.autoAimEnabled;
    }

    public void SetAutoAimStrength(int value)
    {
        if (value >= AUTOAIM_STRENGTH_MIN && value <= AUTOAIM_STRENGTH_MAX)
            settings.autoAimStrength = value;
    }

    public void SubtitlesToggle()
    {
        settings.subtitlesEnabled = !settings.subtitlesEnabled;
    }

    public void SetSubtitleTextSize(int value)
    {
        if (value >= SUBTITLE_MIN_SIZE && value <= SUBTITLE_MAX_SIZE)
            settings.subtitleTextSize = value;
    }

    public void SetSubtitleBackgroundOpacity(int value)
    {
        if (value >= 0 && value <= 10) {
            settings.subtitleBackgroundOpacity = value;
        }
    }

    public void SetInputSensitivity(int value)
    {
        if (value >= INPUT_SENSITIVITY_MIN && value <= INPUT_SENSITIVITY_MAX)
            settings.inputSensitivity = value;
    }

    public void RumbleToggle()
    {
        settings.rumbleEnabled = !settings.rumbleEnabled;
    }

    public void SetRumbleSensitivity(int value)
    {
        if (value >= RUMBLE_SENSITIVITY_MIN && value <= RUMBLE_SENSITIVITY_MAX)
            settings.rumbleSensitivity = value;
    }

    public void InputDelayToggle()
    {
        settings.inputDelayEnabled = !settings.inputDelayEnabled;
    }

    public void SetSoundFXVolume(int value)
    {
        if (value >= SOUND_MIN && value <= SOUND_MAX) {
            settings.soundFXVolume = value;
        }
    }

    public void SetMusicVolume(int value)
    {
        if (value >= SOUND_MIN && value <= SOUND_MAX) {
            settings.musicVolume = value;
        }
    }

    public void SetVoiceVolume(int value)
    {
        if (value >= SOUND_MIN && value <= SOUND_MAX) {
            settings.voiceVolume = value;
        }
    }

    public void SetAudioPlaybackType(int index)
    {
        settings.audioSpeakerMode = (AudioSpeakerMode)index;
    }

    #endregion

    public void ApplyChanges()
    {
        // Resolution and fullscreen
        ApplyResolution(ref settings.currentResolution, ref settings.screenWidth, ref settings.screenHeight, ref settings.fullscreenEnabled);

        // Text Size

        // Audio Playback
        AudioSettings.speakerMode = settings.audioSpeakerMode;

        // Input Delay
        _input.SetInputDelay(settings.inputDelayEnabled);

        // Input Sensitivity
        _input.SetInputSensitivity(settings.inputSensitivity);

        // Rumble Sensitivity
        _input.SetRumbleSensitivity(settings.rumbleSensitivity);
    }

    public void RevertChanges()
    {
        // Resolution and fullscreen
        ApplyResolution(ref currentResolution, ref screenWidth, ref screenHeight, ref fullscreenEnabled);

        // Audio Playback
        AudioSettings.speakerMode = audioSpeakerMode;

        // Input Delay
        _input.SetInputDelay(inputDelayEnabled);

        // Input Sensitivity
        _input.SetInputSensitivity(inputSensitivity);

        // Rumble Sensitivity
        _input.SetRumbleSensitivity(rumbleSensitivity);
    }

    public void UpdateFont()
    {
        var textComponents = Component.FindObjectsOfType<Text>();
        var tmpTextComponents = Component.FindObjectsOfType<TMPro.TMP_Text>();

        if (dyslexicTextEnabled) { // Change all text to use Dyslexic font
            foreach (var component in textComponents)
                component.font = dyslexicFont;

            foreach (var component in tmpTextComponents)
                component.font = dyslexicTMPFont;
        } else { // Change back to original font
            foreach (var component in textComponents) {
                if (component.gameObject.name != "DyslexicText")
                    component.font = classicFont;
            }

            foreach (var component in tmpTextComponents) {
                if (component.gameObject.name != "DyslexicText")
                    component.font = classicTMPFont;
            }
        }
    }

    public void UpdateTextSize()
    {
        //var textComponents = Component.FindObjectsOfType<Text>();
        //var tmpTextComponents = Component.FindObjectsOfType<TMPro.TMP_Text>();

        //if (dyslexicTextEnabled) { // Change all text to use Dyslexic font
        //    foreach (var component in textComponents)
        //        component.fontSize = textSize;

        //    foreach (var component in tmpTextComponents)
        //        component.font = dyslexicTMPFont;
        //} else { // Change back to Arial
        //    foreach (var component in textComponents) {
        //        if (component.gameObject.name != "DyslexicText")
        //            component.font = classicFont;
        //    }

        //    foreach (var component in tmpTextComponents) {
        //        if (component.gameObject.name != "DyslexicText")
        //            component.font = classicTMPFont;
        //    }
        //}
    }

    public void ApplyResolution(ref string currentResolution, ref int screenWidth, ref int screenHeight, ref bool fullscreen)
    {
        var resolutionString = currentResolution;
        string[] values = resolutionString.Split(new string[] { " x " }, StringSplitOptions.RemoveEmptyEntries);

        screenWidth = Int32.Parse(values[0]);
        screenHeight = Int32.Parse(values[1]);

        Screen.SetResolution(screenWidth, screenHeight, fullscreenEnabled);
    }

    public void LoadSettings()
    {
        settings.gameSpeed = gameSpeed;
        settings.autoAimEnabled = autoAimEnabled;
        settings.autoAimStrength = autoAimStrength;

        settings.textSize = textSize;
        settings.fullscreenEnabled = fullscreenEnabled;
        settings.currentResolution = currentResolution;
        settings.subtitlesEnabled = subtitlesEnabled;
        settings.subtitleTextSize = subtitleTextSize;
        settings.subtitleBackgroundOpacity = subtitleBackgroundOpacity;

        settings.inputSensitivity = inputSensitivity;
        settings.rumbleEnabled = rumbleEnabled;
        settings.rumbleSensitivity = rumbleSensitivity;
        settings.inputDelayEnabled = inputDelayEnabled;

        settings.soundFXVolume = soundFXVolume;
        settings.musicVolume = musicVolume;
        settings.voiceVolume = voiceVolume;
        settings.audioSpeakerMode = audioSpeakerMode;
    }

    public void SaveSettings()
    {
        gameSpeed = settings.gameSpeed;
        autoAimEnabled = settings.autoAimEnabled;
        autoAimStrength = settings.autoAimStrength;

        textSize = settings.textSize;
        fullscreenEnabled = settings.fullscreenEnabled;
        currentResolution = settings.currentResolution;
        subtitlesEnabled = settings.subtitlesEnabled;
        subtitleTextSize = settings.subtitleTextSize;
        subtitleBackgroundOpacity = settings.subtitleBackgroundOpacity;

        inputSensitivity = settings.inputSensitivity;
        rumbleEnabled = settings.rumbleEnabled;
        rumbleSensitivity = settings.rumbleSensitivity;
        inputDelayEnabled = settings.inputDelayEnabled;

        soundFXVolume = settings.soundFXVolume;
        musicVolume = settings.musicVolume;
        voiceVolume = settings.voiceVolume;
        audioSpeakerMode = settings.audioSpeakerMode;

        // Apply full screen

        // Apply sound changes
        //AudioSettings.speakerMode = audioSpeakerMode;
    }

    public bool CheckSettings()
    {
        bool speed = gameSpeed == settings.gameSpeed;
        bool autoAim = autoAimEnabled == settings.autoAimEnabled;
        bool strength = autoAimStrength == settings.autoAimStrength;

        bool text = textSize == settings.textSize;
        bool fullscreen = fullscreenEnabled == settings.fullscreenEnabled;
        bool resolution = currentResolution == settings.currentResolution;
        bool subtitles = subtitlesEnabled == settings.subtitlesEnabled;
        bool subTextSize = subtitleTextSize == settings.subtitleTextSize;
        bool background = subtitleBackgroundOpacity == settings.subtitleBackgroundOpacity;

        bool input = inputSensitivity == settings.inputSensitivity;
        bool rumble = rumbleEnabled == settings.rumbleEnabled;
        bool rumbleStr = rumbleSensitivity == settings.rumbleSensitivity;
        bool inputDelay = inputDelayEnabled == settings.inputDelayEnabled;

        bool sound = soundFXVolume == settings.soundFXVolume;
        bool music = musicVolume == settings.musicVolume;
        bool voice = voiceVolume == settings.voiceVolume;
        bool audio = audioSpeakerMode == settings.audioSpeakerMode;

        if (speed && autoAim && strength && text && fullscreen && resolution && subtitles && subTextSize && background && input && rumble && rumbleStr && inputDelay && sound && music && voice && audio)
            return true;

        return true;
    }

    // TODO: Get settings and translate to analytics
    public void UpdateAnalytics()
    {

    }

    [Serializable]
    public class ModifiedSettings
    {
        // General
        public float gameSpeed = 100;
        public bool autoAimEnabled = false;
        public int autoAimStrength = 100;

        // Video
        public int textSize = 28;
        public bool fullscreenEnabled = false;
        public string currentResolution = "800x600";
        public int screenWidth, screenHeight;

        public bool subtitlesEnabled = false;
        public int subtitleTextSize = 42;
        public int subtitleBackgroundOpacity = 10;

        // Input
        public int inputSensitivity = 5;
        public bool rumbleEnabled = true;
        public int rumbleSensitivity = 100;
        public bool inputDelayEnabled = false;

        // Sound
        public int soundFXVolume = 10;
        public int musicVolume = 10;
        public int voiceVolume = 10;
        public AudioSpeakerMode audioSpeakerMode = AudioSpeakerMode.Stereo;
    }
}
