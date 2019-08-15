using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
{
    public override void InstallBindings()
    {
        //Container.Bind<IMenuController>().To<MenuController>().AsSingle().NonLazy();
        Container.Bind<MenuController>().AsSingle().NonLazy();
    }
}