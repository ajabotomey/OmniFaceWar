using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
{
    public MenuController.Settings menuSettings;

    public override void InstallBindings()
    {
        //Container.BindInstance(menuSettings);
        Container.Bind<MenuController>().AsSingle().WithArguments(menuSettings);
    }
}