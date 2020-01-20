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
    [Inject] private GameUIController _gameUI;

    private bool isPushing = false;
    private bool notificationActive = false;

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

    void Start()
    {
        StartCoroutine(PushNotifications());
    }

    void OnEnable()
    {
        StartCoroutine(PushNotifications());
    }

    void Update()
    {
        if (notificationList.Empty())
            return;

        if (notificationList.First().IsFading) {
            notificationList.Pop();
            notificationActive = false;
        }
    }

    public void Push(string title)
    {
        Notification n = _manager.PushNotification(title);

        if (n == null)
            return;

        if (n.Pushed)
            return;

        AddToQueue(n);
    }

    public void Push(Notification n)
    {
        bool result = _manager.CheckNotification(n);

        if (result)
            AddToQueue(n);
    }

    private void AddToQueue(Notification notification)
    {
        // Now we can add the new notification
        NotificationPopup popup = _popupFactory.Create();
        SetupNotification(popup, notification);
        notificationList.Add(popup);
        _settings.UpdateFont();
    }

    private IEnumerator PushNotifications()
    {
        while (true) {

            while (notificationList.Empty())
                yield return new WaitForEndOfFrame();

            while (isPushing) 
                yield return new WaitForEndOfFrame();

            while (_gameUI.IsInteractingWithUI())
                yield return new WaitForEndOfFrame();

            while (notificationActive) // Goes wrong once list if empty. Game still technically works but I'd rather get rid of this error
                yield return new WaitForEndOfFrame();

            if (!notificationList.Empty()) {
                notificationList[0].ShowNotification();
                isPushing = true;
                notificationActive = true;
            }
        }
    }

    private void SetupNotification(NotificationPopup n, Notification notification)
    {
        RectTransform rect = GetComponent<RectTransform>();
        RectTransform popupRect = n.GetComponent<RectTransform>();
        popupRect.SetParent(rect);
        popupRect.localScale = Vector3.one;
        popupRect.localPosition = startPosition;
        n.SetNotification(notification);
        n.SetPosition(firstPosition);
    }

    public void NotificationPushed()
    {
        isPushing = false;
    }
}
