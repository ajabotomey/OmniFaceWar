using UnityEngine;
using Zenject;

public class NotificationInstaller : MonoInstaller
{
    [Header("Popups")]
    [SerializeField] private NotificationPopup popup;
    [SerializeField] private NotificationPopupImage popupImage;

    [Header("Full Notifications")]
    [SerializeField] private NotificationFull fullNotification;
    [SerializeField] private NotificationFullImage imageNotification;

    public override void InstallBindings()
    {
        Container.BindFactory<NotificationPopup, NotificationPopup.Factory>().FromComponentInNewPrefab(popup);
        Container.BindFactory<NotificationPopupImage, NotificationPopupImage.Factory>().FromComponentInNewPrefab(popupImage);
        Container.BindFactory<NotificationFull, NotificationFull.Factory>().FromComponentInNewPrefab(fullNotification);
        Container.BindFactory<NotificationFullImage, NotificationFullImage.Factory>().FromComponentInNewPrefab(imageNotification);
    }
}