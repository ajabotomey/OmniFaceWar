using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class NotificationFull : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text textbox;

    public void CreateNotification(string notificationTitle, string notificationText)
    {
        string parsedText = ParseEmojis.Parse(notificationText);

        title.text = notificationTitle;
        textbox.text = parsedText;
    }

    public class Factory : PlaceholderFactory<NotificationFull> { }
}
