using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MainMenuController mainMenu;
    [SerializeField] SettingsMenuController settingsMenu;
    //[SerializeField] GameObject controlMapperWindow;

    void Start()
    {
        mainMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        //controlMapperWindow.SetActive(false);
    }

    public void SwapToMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        mainMenu.SelectPlayButton();
    }

    public void SwapToSettingsMenu()
    {
        settingsMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        //controlMapperWindow.SetActive(false);
    }

    public void SwapToControlMapper()
    {
        settingsMenu.gameObject.SetActive(false);
        //controlMapperWindow.SetActive(true);
    }
}
