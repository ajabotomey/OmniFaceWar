using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Button
{
    private Navigation ButtonNav { get; set;}

    new void Awake()
    {
        ButtonNav = base.navigation;
    }
}
