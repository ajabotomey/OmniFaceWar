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
    private NotificationFull.Factory _factory;
    private IInputController _inputController;
    private GameUIController _gameUI;

    [Inject] private SettingsManager _settings;

    [Inject]
    public void Construct(NotificationsManager manager, NotificationFull.Factory factory, IInputController inputController, GameUIController gameUI)
    {
        _manager = manager;
        _factory = factory;
        _inputController = inputController;
        _gameUI = gameUI;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _settings.UpdateFont();

        // Make sure all toggles are off
        toggleGroup.SetAllTogglesOff();

        UpdateNotifications();

        tutorialToggle.Select();
        tutorialToggle.OnSelect(null);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if escape or b button is used to exit the window
        if (_inputController.UICancel()) {
            _gameUI.HideNotificationWindow();
        }

        // Do checks on toggles separately
        if (tutorialToggle.isOn) {
            FilterTutorial();
            return;
        } else if (complianceToggle.isOn) {
            FilterCompliance();
            return;
        } else if (emergencyToggle.isOn) {
            FilterEmergency();
            return;
        } else if (newsToggle.isOn) {
            FilterNews();
            return;
        }

        UpdateNotifications();
    }

    private void CreateNotification(Notification n)
    {
        NotificationFull notification = _factory.Create();
        RectTransform rect = notification.GetComponent<RectTransform>();
        //notification.CreateNotification(n.Image, n.NormalText);
        rect.SetParent(scrollView);
        rect.localScale = Vector3.one;

            // Check if image
            // Create image notification
            // else 
            // Create normal notification

            // Make sure to parent to scrollview
        }

    public void UpdateNotifications()
    {
        ClearNotifications();

        // Find out if there are any pushed notifications
        IEnumerable<Notification> pushedNotifications = _manager.GetPushedNotifications();
        List<Notification> notificationList = pushedNotifications.ToList();

        // Now create the notifications
        foreach (Notification n in notificationList) {
            CreateNotification(n);
        }
    }

    public void FilterTutorial()
    {
        ClearNotifications();

        IEnumerable<Notification> pushedNotifications = _manager.GetPushedTutorialNotifications();
        List<Notification> notificationList = pushedNotifications.ToList();

        // Now create the notifications
        foreach (Notification n in notificationList) {
            CreateNotification(n);
        }
    }

    public void FilterCompliance()
    {
        ClearNotifications();

        IEnumerable<Notification> pushedNotifications = _manager.GetPushedComplianceNotifications();
        List<Notification> notificationList = pushedNotifications.ToList();

        // Now create the notifications
        foreach (Notification n in notificationList) {
            CreateNotification(n);
        }
    }

    public void FilterEmergency()
    {
        ClearNotifications();

        IEnumerable<Notification> pushedNotifications = _manager.GetPushedEmergencyNotifications();
        List<Notification> notificationList = pushedNotifications.ToList();

        // Now create the notifications
        foreach (Notification n in notificationList) {
            CreateNotification(n);
        }
    }

    public void FilterNews()
    {
        ClearNotifications();

        IEnumerable<Notification> pushedNotifications = _manager.GetPushedNewsNotifications();
        List<Notification> notificationList = pushedNotifications.ToList();

        // Now create the notifications
        foreach (Notification n in notificationList) {
            CreateNotification(n);
        }
    }

    private void ClearNotifications()
    {
        foreach (Transform child in scrollView) {
            GameObject.Destroy(child.gameObject);
        }
    }
}
