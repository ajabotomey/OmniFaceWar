using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NotificationFull : MonoBehaviour
{
    [SerializeField] private Text textbox;

    public void CreateNotification(string notificationText)
    {
        textbox.text = notificationText;
    }

    public class Factory : PlaceholderFactory<NotificationFull> { }
}
