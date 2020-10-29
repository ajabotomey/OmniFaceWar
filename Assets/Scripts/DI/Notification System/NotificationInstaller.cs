using UnityEngine;
using Zenject;

public class NotificationInstaller : MonoInstaller
{
    [Header("Popups")]
    [SerializeField] private NotificationPopup popup;

    [Header("Full Notifications")]
    [SerializeField] private NotificationFullImage fullNotification;

    public override void InstallBindings()
    {
        Container.BindFactory<NotificationPopup, NotificationPopup.Factory>().FromComponentInNewPrefab(popup);
        Container.BindFactory<NotificationFullImage, NotificationFullImage.Factory>().FromComponentInNewPrefab(fullNotification);
    }
}