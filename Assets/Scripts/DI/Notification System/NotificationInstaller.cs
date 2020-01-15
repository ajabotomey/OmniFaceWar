using UnityEngine;
using Zenject;

public class NotificationInstaller : MonoInstaller
{
    [Header("Popups")]
    [SerializeField] private NotificationPopup popup;

    [Header("Full Notifications")]
    [SerializeField] private NotificationFull fullNotification;

    public override void InstallBindings()
    {
        Container.BindFactory<NotificationPopup, NotificationPopup.Factory>().FromComponentInNewPrefab(popup);
        Container.BindFactory<NotificationFull, NotificationFull.Factory>().FromComponentInNewPrefab(fullNotification);
    }
}