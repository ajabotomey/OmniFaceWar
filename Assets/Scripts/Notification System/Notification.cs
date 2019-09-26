using UnityEngine;

public enum NotificationType
{
    Tutorial, Compliance, Emergency,
}

[CreateAssetMenu(fileName = "New Notification", menuName = "Notification System/New Notification")]
public class Notification : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private Sprite image;
    [SerializeField] [TextArea] private string normalText;
    [SerializeField] private string abbreviatedText; // 88 Characters long (Dyslexic font)
    [SerializeField] private NotificationType type;
    [SerializeField] private bool hasBeenPushed;

    public string Title {
        get { return title; }
        set { title = value; }
    }

    public Sprite Image {
        get { return image; }
        set { image = value; }
    }

    public string NormalText {
        get { return normalText; }
        set { normalText = value; }
    }

    public string AbbreviatedText {
        get { return abbreviatedText; }
        set { abbreviatedText = value; }
    }

    public NotificationType Type {
        get { return type; }
        set { type = value; }
    }

    public bool Pushed {
        get { return hasBeenPushed; }
        set { hasBeenPushed = value; }
    }

    public bool HasImage()
    {
        return image;
    }
}
