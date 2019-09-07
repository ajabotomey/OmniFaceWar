using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [Header("Player and Gadgets")]
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject NoisemakerPrefab;

    [Header("Game UI")]
    [SerializeField] private GameUIController gameUIController;

    [Header("Heatmap")]
    [SerializeField] private HeatmapUploadController heatmap;

    public override void InstallBindings()
    {
        // Bind Heatmap controller
        Container.Bind<HeatmapUploadController>().FromInstance(heatmap).AsSingle().NonLazy();

        // Bind UI
        Container.Bind<GameUIController>().FromInstance(gameUIController).AsSingle();

        // Bind player and gadgets
        Container.Bind<PlayerControl>().FromInstance(playerControl);
        Container.BindFactory<Bullet, Bullet.Factory>().FromComponentInNewPrefab(BulletPrefab);
        Container.BindFactory<Noisemaker, Noisemaker.Factory>().FromComponentInNewPrefab(NoisemakerPrefab);
    }
}