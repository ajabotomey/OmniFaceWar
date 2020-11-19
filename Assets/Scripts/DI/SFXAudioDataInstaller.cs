using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SFX Audio Data Installer", menuName = "Installers/SFX Audio Data Installer")]
public class SFXAudioDataInstaller : ScriptableObjectInstaller<SFXAudioDataInstaller>
{
    [SerializeField] private SFXAudioData data;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SFXAudioData>().FromInstance(data).AsSingle();
    }
}