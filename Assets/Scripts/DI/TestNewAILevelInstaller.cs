using UnityEngine;
using Zenject;

public class TestNewAILevelInstaller : MonoInstaller
{
    [Header("Player and Gadgets")]
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private GameObject NoisemakerPrefab;

    [Header("Game UI")]
    [SerializeField] private GameUIController gameUIController;

    [Header("Heatmap")]
    [SerializeField] private HeatmapUploadController heatmap;

    [Header("Objectives Manager")]
    [SerializeField] private ObjectivesManager objectivesManager;

    public override void InstallBindings()
    {
        // Bind Heatmap controller
        Container.Bind<HeatmapUploadController>().FromInstance(heatmap).AsSingle().NonLazy();

        // Bind UI
        Container.Bind<GameUIController>().FromInstance(gameUIController).AsSingle();

        // Bind Objectives manager
        Container.Bind<ObjectivesManager>().FromInstance(objectivesManager).AsSingle();

        // Bind player and gadgets
        Container.Bind<PlayerControl>().FromInstance(playerControl).AsSingle();
        Container.BindFactory<Bullet, Bullet.Factory>().FromComponentInNewPrefab(BulletPrefab);
        Container.BindFactory<NoisemakerObject, NoisemakerObject.Factory>().FromComponentInNewPrefab(NoisemakerPrefab);
    }
}