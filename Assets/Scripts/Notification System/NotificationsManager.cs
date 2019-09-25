using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

// Holds all of the notifications
[CreateAssetMenu(fileName = "Notifications Manager", menuName = "Notification System/Notification Manager")]
public class NotificationsManager : ScriptableObject
{

    [SerializeField] private List<Notification> notifications;
    [SerializeField] private List<Notification> pushedNotifications;

    public Notification PushNotification(string title)
    {
        bool notificationFound = false;
        bool pushed = false;
        Notification notification = new Notification();

        // Check if title is in notification array and if so, has it been pushed
        foreach (Notification n in notifications) {
            if (n.Title == title) {
                notificationFound = true;
                if (!n.Pushed) {
                    notification = n;
                } else {
                    pushed = true;
                }

                break;
            }
        }

        if (!notificationFound)
            return null;

        if (pushed)
            return null;

        pushedNotifications.Add(notification);
        return notification;
    }

    // Returns true is notification exists and it hasn't been pushed
    public bool CheckNotification(Notification notification)
    {
        // Check if title is in notification array and if so, has it been pushed
        foreach (Notification n in notifications) {
            if (n.Title == notification.Title) {
                if (!n.Pushed) {
                    return true;
                }

                break;
            }
        }

        return false;
    }

    public IEnumerable<Notification> GetPushedNotifications()
    {
        var query =
            from n in notifications
            where n.Pushed == true
            select n;

        return query;
    }

    public IEnumerable<Notification> GetPushedTutorialNotifications()
    {
        var query =
            from n in notifications
            where n.Pushed == true
            where n.Type == NotificationType.Tutorial
            select n;

        return query;
    }

    public IEnumerable<Notification> GetPushedComplianceNotifications()
    {
        var query =
            from n in notifications
            where n.Pushed == true
            where n.Type == NotificationType.Compliance
            select n;

        return query;
    }

    public IEnumerable<Notification> GetPushedEmergencyNotifications()
    {
        var query =
            from n in notifications
            where n.Pushed == true
            where n.Type == NotificationType.Emergency
            select n;

        return query;
    }
}
