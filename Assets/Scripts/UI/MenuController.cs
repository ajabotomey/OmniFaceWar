using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController
{
    readonly Settings _settings;

    public MenuController(Settings settings)
    {
        _settings = settings;

        _settings.mainMenu.gameObject.SetActive(true);
        _settings.settingsMenu.gameObject.SetActive(false);
        _settings.controlMapperWindow.SetActive(false);
    }

    public void SwapToMainMenu()
    {
        _settings.mainMenu.gameObject.SetActive(true);
        _settings.settingsMenu.gameObject.SetActive(false);
    }

    public void SwapToSettingsMenu()
    {
        _settings.settingsMenu.gameObject.SetActive(true);
        _settings.mainMenu.gameObject.SetActive(false);
        _settings.controlMapperWindow.SetActive(false);
    }

    public void SwapToControlMapper()
    {
        _settings.settingsMenu.gameObject.SetActive(false);
        _settings.controlMapperWindow.SetActive(true);
    }

    public void ReturnFromControlMapper()
    {
        SwapToSettingsMenu();
        _settings.settingsMenu.GetComponent<SettingsMenuController>().ReturnFromControlMapper();
    }

    [System.Serializable]
    public class Settings
    {
        public MainMenuController mainMenu;
        public SettingsMenuController settingsMenu;
        public GameObject controlMapperWindow;
    }
}
