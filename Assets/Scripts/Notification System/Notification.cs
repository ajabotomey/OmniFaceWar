﻿using UnityEngine;

public enum NotificationType
{
    Tutorial, Compliance, Emergency, News
}

[CreateAssetMenu(fileName = "New Notification", menuName = "Notification System/New Notification")]
public class Notification : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private Sprite image;
    [SerializeField] [TextArea] private string normalText;
    [SerializeField] [TextArea] private string abbreviatedText; // 88 Characters long (Dyslexic font)
    [SerializeField] private NotificationType type;
    [SerializeField] private bool hasBeenPushed;
    //[SerializeField] [ActionIdProperty(typeof(RewiredConsts.Action))] public int rewiredAction;
    [SerializeField] private string inputBinding;

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

    public string InputBinding
    {
        get { return inputBinding; }
    }

    public int RewiredAction {
        get { /*return rewiredAction;*/ return 0; }
    }

    public bool HasImage()
    {
        return image;
    }

    public bool IsQueued
    {
        get; set;
    }
}
