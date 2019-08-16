using UnityEngine;
using Zenject;

public class MenuControlInstaller : MonoInstaller<MainMenuController, SettingsMenuController, GameObject, MenuControlInstaller>
{
    private MainMenuController mainMenu;
    private SettingsMenuController settingsMenu;
    private GameObject controlMapperWindow;

    [Inject]
    public void Construct(MainMenuController main, SettingsMenuController settings, GameObject control)
    {
        mainMenu = main;
        settingsMenu = settings;
        controlMapperWindow = control;
    }

    public override void InstallBindings()
    {
        Container.Bind<MenuController>().AsSingle().NonLazy();
        Container.BindInstance(mainMenu).WhenInjectedInto<MenuController>();
        Container.BindInstance(settingsMenu).WhenInjectedInto<MenuController>();
        Container.BindInstance(controlMapperWindow).WhenInjectedInto<MenuController>();
    }
}