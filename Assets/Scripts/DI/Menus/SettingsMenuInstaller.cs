using UnityEngine;
using Zenject;

public class SettingsMenuInstaller : MonoInstaller<SettingsMenuInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<SettingsMenuController>().AsSingle().NonLazy();
    }
}