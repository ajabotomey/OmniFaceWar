using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private MainMenuController mainMenu;
    //[SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private GameObject controlMapperWindow;
    
    void Awake()
    {
        mainMenu.gameObject.SetActive(true);
        //settingsMenu.gameObject.SetActive(false);
        controlMapperWindow.SetActive(false);
    }

    public void SwapToMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        //settingsMenu.gameObject.SetActive(false);
    }

    public void SwapToSettingsMenu()
    {
        //settingsMenu.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        controlMapperWindow.SetActive(false);
    }

    public void SwapToControlMapper()
    {
        //settingsMenu.gameObject.SetActive(false);
        controlMapperWindow.SetActive(true);
    }

    public void ReturnFromControlMapper()
    {
        SwapToSettingsMenu();
        //settingsMenu.GetComponent<SettingsMenu>().ReturnFromControlMapper();
    }
}
