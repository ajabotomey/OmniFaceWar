﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShowControllerButtonUI : MonoBehaviour
{
    [SerializeField] private ControllerActionButton action;
    [SerializeField] private Image image;

    [Inject]
    private IInputController inputController;

    // Update is called once per frame
    void Update()
    {
        DeviceType deviceType = inputController.GetControllerType();

        switch (deviceType) {
            case DeviceType.PC:
                image.sprite = action.pcImage;
                break;
            case DeviceType.XboxOne:
            case DeviceType.Xbox360:
                image.sprite = action.xboxImage;
                break;
            case DeviceType.PS4:
                image.sprite = action.playstationImage;
                break;
        }
    }
}