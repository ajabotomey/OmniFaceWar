using UnityEngine;
using Zenject;

public class TestAILevelInstaller : MonoInstaller
{
    [Header("Player and Gadgets")]
    [SerializeField] private TestPlayerControl playerControl;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject NoisemakerPrefab;

    public override void InstallBindings()
    {
        // Bind player and gadgets
        Container.Bind<TestPlayerControl>().FromInstance(playerControl).AsSingle();
        Container.BindFactory<Bullet, Bullet.Factory>().FromComponentInNewPrefab(BulletPrefab);
        Container.BindFactory<Noisemaker, Noisemaker.Factory>().FromComponentInNewPrefab(NoisemakerPrefab);
    }
}