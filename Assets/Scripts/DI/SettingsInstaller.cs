using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    [SerializeField]
    private SettingsManager settingsManager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SettingsManager>().FromInstance(settingsManager).AsSingle();
    }
}