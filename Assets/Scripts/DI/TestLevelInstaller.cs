using UnityEngine;
using Zenject;

public class TestLevelInstaller : MonoInstaller
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject NoisemakerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<PlayerControl>().FromInstance(player).AsSingle();
        Container.BindFactory<Bullet, Bullet.Factory>().FromComponentInNewPrefab(BulletPrefab);
        Container.BindFactory<NoisemakerObject, NoisemakerObject.Factory>().FromComponentInNewPrefab(NoisemakerPrefab);
    }
}