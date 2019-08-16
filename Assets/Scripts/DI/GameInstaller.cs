using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IInputController>().To<InputController>().AsSingle().CopyIntoAllSubContainers().NonLazy();
    }
}