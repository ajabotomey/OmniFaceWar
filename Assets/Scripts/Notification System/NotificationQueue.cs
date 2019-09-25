using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NotificationQueue : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform firstSpot;
    [SerializeField] private Transform secondSpot;
    [SerializeField] private Transform thirdSpot;
    [SerializeField] private Transform offScreenSpot;

    private Vector2 firstPosition;
    private Vector2 secondPosition;
    private Vector2 thirdPosition;
    private Vector2 startPosition;

    private List<NotificationPopup> notificationList; // 3rd in the list gets the first spot, etc...

    // Start is called before the first frame update
    private NotificationsManager _manager;
    private NotificationPopup.Factory _popupFactory;
    private NotificationPopupImage.Factory _imageFactory;

    [Inject]
    public void Construct(NotificationsManager manager, NotificationPopup.Factory popupFactory, NotificationPopupImage.Factory imageFactory)
    {
        _manager = manager;
        _popupFactory = popupFactory;
        _imageFactory = imageFactory;
    }

    void Start()
    {
        notificationList = new List<NotificationPopup>();
        firstPosition = firstSpot.position;
        secondPosition = secondSpot.position;
        thirdPosition = thirdSpot.position;
        startPosition = offScreenSpot.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Push(string title)
    {
        Notification n = _manager.PushNotification(title);

        if (n == null)
            return;

        if (n.Pushed)
            return;

        PushNotification(n);
    }

    public void Push(Notification n)
    {
        bool result = _manager.CheckNotification(n);

        if (result)
            PushNotification(n);
    }

    private void PushNotification(Notification notification)
    {
        // Check the number of notifications
        if (notificationList.Count > 0) {
            // OK, we have some work to do before adding anything

            if (notificationList.Count == 3) {
                // Remove the oldest popup
                notificationList[0].Disappear();
                notificationList.RemoveAt(0);
            }

            // Now move the notifications
            if (notificationList.Count == 2) { // If we now have 2 notifications
                notificationList[1].SetPosition(secondPosition);
                notificationList[0].SetPosition(thirdPosition);
            } else {
                notificationList[0].SetPosition(secondPosition);
            }
        }

        // Now we can add the new notification
        if (notification.HasImage()) {
            NotificationPopupImage popup = _imageFactory.Create();
            popup.transform.SetParent(transform);
            popup.transform.localScale = Vector3.one;
            popup.transform.position = startPosition;
            popup.SetPosition(firstPosition);
            popup.ShowNotification(notification);
            notificationList.Add(popup);
        } else {
            NotificationPopup popup = _popupFactory.Create();
            popup.transform.SetParent(transform);
            popup.transform.localScale = Vector3.one;
            popup.transform.position = startPosition;
            popup.SetPosition(firstPosition);
            popup.ShowNotification(notification);
            notificationList.Add(popup);
        }
    }
}
