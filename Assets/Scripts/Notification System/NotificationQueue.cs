using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NotificationQueue : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform firstSpot;
    [SerializeField] private Transform offScreenSpot;

    private Vector2 firstPosition;
    private Vector2 startPosition;

    private List<NotificationPopup> notificationList; // 3rd in the list gets the first spot, etc...

    // Start is called before the first frame update
    private NotificationsManager _manager;
    private NotificationPopup.Factory _popupFactory;

    [Inject] private SettingsManager _settings;

    private bool isPushing = false;

    [Inject]
    public void Construct(NotificationsManager manager, NotificationPopup.Factory popupFactory)
    {
        _manager = manager;
        _popupFactory = popupFactory;
    }

    void Awake()
    {
        notificationList = new List<NotificationPopup>();
        firstPosition = firstSpot.localPosition;
        startPosition = offScreenSpot.localPosition;
    }

    void Update()
    {
        if (notificationList.First().IsFading) {
            notificationList.Pop();
        }
    }

    public void Push(string title)
    {
        Notification n = _manager.PushNotification(title);

        if (n == null)
            return;

        if (n.Pushed)
            return;

        //PushNotification(n);
        StartCoroutine(PushNotification(n));
    }

    public void Push(Notification n)
    {
        bool result = _manager.CheckNotification(n);

        if (result)
            StartCoroutine(PushNotification(n));
    }

    private IEnumerator PushNotification(Notification notification)
    {
        // Check if a notification is in the middle of pushing
        while (isPushing) {
            yield return new WaitForEndOfFrame();
        }

        // Check the number of notifications
        while (notificationList.Count > 0) {
            // OK, we have some work to do before adding anything

            //if (notificationList.Count == 3) {
            //    // Remove the oldest popup
            //    notificationList[0].Disappear();
            //    notificationList.RemoveAt(0);
            //}

            //// Now move the notifications
            //if (notificationList.Count == 2) { // If we now have 2 notifications
            //    notificationList[1].SetPosition(secondPosition);
            //    notificationList[0].SetPosition(thirdPosition);
            //} else {
            //    notificationList[0].SetPosition(secondPosition);
            //}

            yield return new WaitForEndOfFrame();
        }

        // Now we can add the new notification
        NotificationPopup popup = _popupFactory.Create();
        SetupNotification(popup, notification);
        popup.ShowNotification(notification);
        notificationList.Add(popup);
        isPushing = true;

        _settings.UpdateFont();

        yield return new WaitForEndOfFrame();
    }

    private void SetupNotification(NotificationPopup n, Notification notification)
    {
        RectTransform rect = GetComponent<RectTransform>();
        RectTransform popupRect = n.GetComponent<RectTransform>();
        popupRect.SetParent(rect);
        popupRect.localScale = Vector3.one;
        popupRect.localPosition = startPosition;
        n.SetPosition(firstPosition);
    }

    public void NotificationPushed()
    {
        isPushing = false;
    }
}
