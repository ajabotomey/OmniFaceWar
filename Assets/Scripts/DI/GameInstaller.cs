using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        BaseGameInstaller.Install(Container);
    }
}

public class BaseGameInstaller : Installer<BaseGameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IInputController>().To<InputController>().AsSingle().CopyIntoAllSubContainers().NonLazy();
    }
}