using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Rewired Action Manager Installer", menuName = "Installers/Rewired Action Manager Installer")]
public class RewiredActionManagerInstaller : ScriptableObjectInstaller<RewiredActionManagerInstaller>
{
    [SerializeField] private RewiredActionManager manager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<RewiredActionManager>().FromInstance(manager).AsSingle();
    }
}