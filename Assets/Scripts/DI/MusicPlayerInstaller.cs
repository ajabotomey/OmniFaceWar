using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Music Player Installer", menuName = "Installers/Music Player Installer")]
public class MusicPlayerInstaller : ScriptableObjectInstaller<MusicPlayerInstaller>
{
    [SerializeField] private MusicPlayer musicPlayer;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MusicPlayer>().FromInstance(musicPlayer).AsSingle();
    }
}