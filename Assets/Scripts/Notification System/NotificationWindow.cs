using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Zenject;

public class NotificationWindow : MonoBehaviour
{
    [SerializeField] private List<NotificationFull> displayedNotifications;

    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Toggle tutorialToggle;
    [SerializeField] private Toggle complianceToggle;
    [SerializeField] private Toggle emergencyToggle;
    [SerializeField] private Toggle newsToggle;

    [SerializeField] private RectTransform scrollView;

    private NotificationsManager _manager;
    private NotificationFullImage.Factory _factory;
    private GameUIController _gameUI;

    [Inject] private SettingsManager _settings;

    [Inject]
    public void Construct(NotificationsManager manager, NotificationFullImage.Factory factory, GameUIController gameUI)
    {
        _manager = manager;
        _factory = factory;
        _gameUI = gameUI;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _settings.UpdateFont();

        // Make sure all toggles are off
        toggleGroup.SetAllTogglesOff();

        FilterNotifications();

        tutorialToggle.Select();
        tutorialToggle.OnSelect(null);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    FilterNotifications();
    //}

    private void CreateNotification(Notification n)
    {
        NotificationFullImage notification = _factory.Create();
        RectTransform rect = notification.GetComponent<RectTransform>();
        notification.CreateNotification(n.Image, n.Title, n.NormalText);
        rect.SetParent(scrollView);
        rect.localScale = Vector3.one;
    }

    private void ClearNotifications()
    {
        foreach (Transform child in scrollView) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void FilterNotifications()
    {
        ClearNotifications();

        IEnumerable<Notification> pushedNotifications = Enumerable.Empty<Notification>();
        
        if (tutorialToggle.isOn) {
            pushedNotifications = _manager.GetPushedTutorialNotifications();
        }

        if (complianceToggle.isOn) {
            var result = _manager.GetPushedComplianceNotifications();

            if (pushedNotifications == Enumerable.Empty<Notification>()) {
                pushedNotifications = result;
            }
            else {
                var union = pushedNotifications.Union(result);
                pushedNotifications = union;
            }
        }

        if (emergencyToggle.isOn) {
            var result = _manager.GetPushedEmergencyNotifications();

            if (pushedNotifications == Enumerable.Empty<Notification>()) {
                pushedNotifications = result;
            } else {
                var union = pushedNotifications.Union(result);
                pushedNotifications = union;
            }
        }

        if (newsToggle.isOn) {
            var result = _manager.GetPushedNewsNotifications();

            if (pushedNotifications == Enumerable.Empty<Notification>()) {
                pushedNotifications = result;
            } else {
                var union = pushedNotifications.Union(result);
                pushedNotifications = union;
            }
        }

        // If still empty, display all notifications
        if (pushedNotifications == Enumerable.Empty<Notification>()) {
            pushedNotifications = _manager.GetPushedNotifications();
        }

        List<Notification> notificationList = pushedNotifications.ToList();
        

        // Now create the notifications
        foreach (Notification n in notificationList) {
            CreateNotification(n);
        }

    }
}
