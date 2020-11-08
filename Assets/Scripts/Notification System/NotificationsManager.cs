using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Holds all of the notifications
[CreateAssetMenu(fileName = "Notifications Manager", menuName = "Notification System/Create Notification Manager")]
public class NotificationsManager : ScriptableObject
{
    [SerializeField] private List<Notification> notifications;

    public Notification PushNotification(string title)
    {
        Notification notification = ScriptableObject.CreateInstance<Notification>();

        // Check if title is in notification array and if so, has it been pushed
        foreach (Notification n in notifications) {
            if (n.Title == title) {
                //if (!n.Pushed) {
                    return n;
                //}

                break;
            }
        }

        return null;
    }

    // Returns true is notification exists and it hasn't been pushed
    public bool CheckNotification(Notification notification)
    {
        // Check if title is in notification array and if so, has it been pushed
        foreach (Notification n in notifications) {
            if (n.Title == notification.Title) {
                //if (!n.Pushed)
                    return true; // Modify so we can see the notification again but not in the Notification Window
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

    public IEnumerable<Notification> GetPushedNewsNotifications()
    {
        var query =
            from n in notifications
            where n.Pushed == true
            where n.Type == NotificationType.News
            select n;

        return query;
    }
}
