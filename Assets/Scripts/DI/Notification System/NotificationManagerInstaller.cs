using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Notifications Manager Installer", menuName = "Installers/Notifications Manager Installer")]
public class NotificationsManagerInstaller : ScriptableObjectInstaller<NotificationsManagerInstaller>
{
    [SerializeField] private NotificationsManager manager;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<NotificationsManager>().FromInstance(manager).AsSingle();
    }
}
