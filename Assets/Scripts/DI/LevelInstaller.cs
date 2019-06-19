using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    public GameObject BulletPrefab;
    public GameObject NoisemakerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<PlayerControl>().FromInstance(FindObjectOfType<PlayerControl>());
        Container.BindFactory<Bullet, Bullet.Factory>().FromComponentInNewPrefab(BulletPrefab);
        Container.BindFactory<Noisemaker, Noisemaker.Factory>().FromComponentInNewPrefab(NoisemakerPrefab);
    }
}