﻿using System.Collections;
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

    [SerializeField] private Transform scrollView;

    private NotificationsManager _manager;
    private NotificationFull.Factory _factory;
    private NotificationFullImage.Factory _imageFactory;

    [Inject]
    public void Construct(NotificationsManager manager, NotificationFull.Factory factory, NotificationFullImage.Factory imageFactory)
    {
        _manager = manager;
        _factory = factory;
        _imageFactory = imageFactory;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        // Make sure all toggles are off
        toggleGroup.SetAllTogglesOff();

        UpdateNotifications();

        tutorialToggle.Select();
        tutorialToggle.OnSelect(null);
    }

    // Update is called once per frame
    void Update()
    {
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
        }

        UpdateNotifications();
    }

    private void CreateNotification(Notification n)
    {
        if (n.Image) {
            NotificationFullImage notification = _imageFactory.Create();
            notification.CreateNotification(n.Image, n.NormalText);
            notification.transform.SetParent(scrollView);
            notification.transform.localScale = Vector3.one;
        } else {
            NotificationFull notification = _factory.Create();
            notification.CreateNotification(n.NormalText);
            notification.transform.SetParent(scrollView);
            notification.transform.localScale = Vector3.one;
        }

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

    private void ClearNotifications()
    {
        foreach (Transform child in scrollView) {
            GameObject.Destroy(child.gameObject);
        }
    }
}