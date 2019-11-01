using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Rewired;
using Zenject;

[System.Serializable]
public struct ControllerActionButton
{
    [ActionIdProperty(typeof(RewiredConsts.Action))]
    public int rewiredAction;
    public Sprite pcImage;
    public Sprite xboxImage;
    public Sprite playstationImage;
}

public class ShowControllerButton : MonoBehaviour
{
    [SerializeField] private ControllerActionButton action;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        DeviceType deviceType = DeviceDictionary.GetControllerType();

        switch (deviceType) {
            case DeviceType.PC:
                spriteRenderer.sprite = action.pcImage;
                break;
            case DeviceType.XboxOne:
            case DeviceType.Xbox360:
                spriteRenderer.sprite = action.xboxImage;
                break;
            case DeviceType.PS4:
                spriteRenderer.sprite = action.playstationImage;
                break;
        }
    }
}
