using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "NotificationManagerInstaller", menuName = "Installers/NotificationManagerInstaller")]
public class NotificationManagerInstaller : ScriptableObjectInstaller<NotificationManagerInstaller>
{
    [SerializeField] private NotificationsManager manager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<NotificationsManager>().FromInstance(manager).AsSingle();
    }
}