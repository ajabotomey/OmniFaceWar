using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SceneInstaller", menuName = "Installers/SceneInstaller")]
public class SceneInstaller : ScriptableObjectInstaller<SceneInstaller>
{
    [SerializeField]
    private SceneController sceneController;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneController>().FromInstance(sceneController).AsSingle();
    }
}