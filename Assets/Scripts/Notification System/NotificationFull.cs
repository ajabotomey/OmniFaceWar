using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class NotificationFull : MonoBehaviour
{
    [SerializeField] private TMP_Text textbox;

    public void CreateNotification(string notificationText)
    {
        string parsedText = ParseEmojis.Parse(notificationText);

        textbox.text = parsedText;
    }

    public class Factory : PlaceholderFactory<NotificationFull> { }
}
