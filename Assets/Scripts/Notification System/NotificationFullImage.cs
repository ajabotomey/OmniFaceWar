﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NotificationFullImage : NotificationFull
{
    [SerializeField] private Image image;

    public void CreateNotification(Sprite sprite, string title, string text)
    {
        image.sprite = sprite;
        base.CreateNotification(title, text);
    }

    new public class Factory : PlaceholderFactory<NotificationFullImage> { }
}
