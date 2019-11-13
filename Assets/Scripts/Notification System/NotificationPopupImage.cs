using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NotificationPopupImage : NotificationPopup
{
    [SerializeField] private Image image;

    [Inject] private RewiredActionManager rewiredActionManager;
    private Notification notification;

    new public void ShowNotification(Notification notification)
    {
        if (notification.RewiredAction == -1) { // No action here
            image.sprite = notification.Image;
        } else { // We have an action
            image.sprite = rewiredActionManager.GetSpriteFromAction(notification.RewiredAction);
        }

        this.notification = notification;

        base.ShowNotification(notification);
    }

    new void Update()
    {
        base.Update();

        if (notification != null)
            image.sprite = rewiredActionManager.GetSpriteFromAction(notification.RewiredAction);
    }

    new public class Factory : PlaceholderFactory<NotificationPopupImage> { }
}