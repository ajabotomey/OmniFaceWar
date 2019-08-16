using UnityEngine;
using Zenject;

public class SettingsMenuInstaller : MonoInstaller<SettingsMenuInstaller>
{
    public override void InstallBindings()
    {
        SettingsBaseInstaller.Install(Container);
    }
}

public class SettingsBaseInstaller : Installer<SettingsBaseInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<SettingsMenuController>().AsSingle().NonLazy();
    }
}