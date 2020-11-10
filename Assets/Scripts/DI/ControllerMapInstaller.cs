using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ControllerMapInstaller", menuName = "Installers/ControllerMapInstaller")]
public class ControllerMapInstaller : ScriptableObjectInstaller<ControllerMapInstaller>
{
    [SerializeField] private ControllerMap map;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ControllerMap>().FromInstance(map).AsSingle();
    }
}