using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NotificationPopupImage : NotificationPopup
{
    [SerializeField] private Image image;

    new public void ShowNotification(Notification notification)
    {
        image.sprite = notification.Image;

        base.ShowNotification(notification);
    }

    new public class Factory : PlaceholderFactory<NotificationPopupImage> { }
}