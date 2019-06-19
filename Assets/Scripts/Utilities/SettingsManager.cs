using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsManager
{
    bool IsDyslexicTextEnabled();
    bool IsFullscreenEnabled();
    float CurrentGameSpeed();
    List<string> ResolutionsSupported();
    bool IsAutoAimEnabled();
    int GetAutoAimStrength();
    bool IsSubtitlesEnabled();
    int GetSubtitlesTextSize();
    int GetTextSize();
    int GetInputSensitivity();
    bool IsRumbleEnabled();
    int GetRumbleSensitivity();
    int GetWindowWidth();
    int GetWindowHeight();
}

public class SettingsManager
{
    public void Construct()
    {

    }


}
