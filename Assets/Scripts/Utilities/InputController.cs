using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public interface IInputController
{
    bool IsControllerActive();
    DeviceType GetControllerType();
    Vector2 MousePosition();
    Vector3 MouseMovement();
    float Horizontal();
    float Vertical();
    Vector2 Rotation();
    Vector3 Rotation3Raw();
    float RotationAtan();
    bool FireWeapon();
    bool SmokeBomb();
    bool SelectWeapon();
    void SetRumble(float duration);
    void StopRumble();
    bool UICancel();
    bool Pause();
    bool TakeScreenshot();
    bool SelectGun();
    bool SelectNoise();
    bool SelectMask();
    float ScrollWeapons();
    bool TalkToNPC();
    bool OpenNotificationWindow();
    void SetAim(Vector2 _aim);
    Vector2 GetAim();
}

public enum DeviceType
{
    PC, XboxOne, Xbox360, PS4
}

public class InputController : IInputController
{
    private int playerID = 0;
    private Player player;

    // Controller Maps
    private Mouse mouse;
    private Joystick joystick;

    Vector2 aim;

    private Dictionary<DeviceType, Guid> deviceDictionary = new Dictionary<DeviceType, Guid> {
        { DeviceType.XboxOne, Guid.Parse("19002688-7406-4f4a-8340-8d25335406c8") },
        { DeviceType.Xbox360, Guid.Parse("d74a350e-fe8b-4e9e-bbcd-efff16d34115") },
        { DeviceType.PS4, Guid.Parse("cd9718bf-a87a-44bc-8716-60a0def28a9f") },
    };

    //// GUID Identifiers for controllers
    //private Guid xboxOneControllerGUID = Guid.Parse("19002688-7406-4f4a-8340-8d25335406c8");
    //private Guid xbox360ControllerGUID = Guid.Parse("d74a350e-fe8b-4e9e-bbcd-efff16d34115");
    //private Guid playstationControllerGUID = Guid.Parse("cd9718bf-a87a-44bc-8716-60a0def28a9f");
    //private Guid nintendoSwitchLeftJoyconGUID = Guid.Parse("3eb01142-da0e-4a86-8ae8-a15c2b1f2a04");
    //private Guid nintendoSwitchRightJoyconGUID = Guid.Parse("605dc720-1b38-473d-a459-67d5857aa6ea");
    //private Guid nintendoSwitchDualJoyconGUID = Guid.Parse("521b808c-0248-4526-bc10-f1d16ee76bf1");
    //private Guid nintendoSwitchHandheldJoyconGUI = Guid.Parse("1fbdd13b-0795-4173-8a95-a2a75de9d204");
    //private Guid nintendoSwitchProController = Guid.Parse("7bf3154b-9db8-4d52-950f-cd0eed8a5819");

    public InputController()
    {
        player = ReInput.players.GetPlayer(playerID);

        if (player.controllers.Mouse.isConnected)
            mouse = ReInput.controllers.Mouse;

        // Check which input should be used.
        if (player.controllers.Joysticks.Count == 1) {
            joystick = player.controllers.Joysticks[0]; // Only ever be one joystick

            // By Default, disable the keyboard and mouse if a controller is connected
            player.controllers.Keyboard.enabled = false;
            player.controllers.Mouse.enabled = false;
        } else {
            joystick = null;

            // If no controller is connected, then ensure that the keyboard are enabled
            player.controllers.Keyboard.enabled = true;
            player.controllers.Mouse.enabled = true;
        }

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    public bool IsControllerActive()
    {
        //return player.controllers.joystickCount == 1;
        Controller controller = player.controllers.GetLastActiveController();
        if (controller != null) {
            switch (controller.type) {
                case ControllerType.Keyboard:
                    return false;
                case ControllerType.Joystick:
                    return true;
                case ControllerType.Mouse:
                    return false;
                case ControllerType.Custom:
                    // Do something custom controller
                    break;
            }
        }

        return false;
    }

    public DeviceType GetControllerType()
    {
        Controller controller = player.controllers.GetLastActiveController();
        if (controller == null)
            return DeviceType.PC;

        Guid controllerGUID = controller.hardwareTypeGuid;

        foreach (KeyValuePair<DeviceType, Guid> device in deviceDictionary) {
            if (device.Value == controllerGUID)
                return device.Key;
        }

        return DeviceType.PC;
    }

    public Vector2 MousePosition()
    {
        return mouse.screenPosition;
    }

    public Vector3 MouseMovement()
    {
        return new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0f);
    }

    public float Horizontal()
    {
        return player.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        return player.GetAxis("Vertical");
    }

    public bool FireWeapon()
    {
        return player.GetButton("UseGadget");
    }

    public bool SmokeBomb()
    {
        return player.GetButton("UseSmokebomb");
    }

    public Vector2 Rotation()
    {
        Vector2 rotateRaw = new Vector2(player.GetAxis("RotateHorizontal"), player.GetAxis("RotateVertical"));
        rotateRaw.Normalize();
        return rotateRaw;
    }

    public Vector3 Rotation3Raw()
    {
        Vector3 rotateRaw = new Vector3(player.GetAxis("RotateHorizontal"), player.GetAxis("RotateVertical"), 0.0f);
        return rotateRaw;
    }

    public float RotationAtan()
    {
        Vector2 rotateRaw = new Vector2(player.GetAxis("RotateVertical"), player.GetAxis("RotateHorizontal"));
        rotateRaw.Normalize();
        return Mathf.Atan2(rotateRaw.x, rotateRaw.y) * Mathf.Rad2Deg;
    }

    public bool SelectWeapon()
    {
        return player.GetButton("GadgetWheel");
    }

    public void SetRumble(float duration)
    {
        if (joystick == null)
            return;

        //var sensitivity = SettingsManager.Instance.GetRumbleSensitivity();
        var sensitivity = 100f;

        if (!joystick.supportsVibration) return;
        for (int i = 0; i < joystick.vibrationMotorCount; i++) {
            joystick.SetVibration(i, sensitivity, duration);
        }
    }

    public void StopRumble()
    {
        joystick.StopVibration();
    }

    public bool UICancel()
    {
        return player.GetButtonDown("UICancel");
    }

    public bool Pause()
    {
        return player.GetButtonDown("Pause");
    }

    public bool TakeScreenshot()
    {
        return player.GetButtonDown("Screenshot");
    }

    public bool SelectGun()
    {
        return player.GetButtonDown("SelectGun");
    }

    public bool SelectNoise()
    {
        return player.GetButtonDown("SelectNoise");
    }

    public bool SelectMask()
    {
        return player.GetButtonDown("SelectMask");
    }

    public float ScrollWeapons()
    {
        return player.GetAxis("ScrollWeapons");
    }

    public bool TalkToNPC()
    {
        return player.GetButtonDown("TalkToNPC");
    }

    public void SetAim(Vector2 _aim)
    {
        aim = _aim;
    }

    public Vector2 GetAim()
    {
        return aim;
    }

    public bool OpenNotificationWindow()
    {
        return player.GetButtonDown("OpenNotificationWindow");
    }

    private void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        player.controllers.Keyboard.enabled = false;
        player.controllers.Mouse.enabled = false;
    }

    private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        player.controllers.Keyboard.enabled = true;
        player.controllers.Mouse.enabled = true;
    }
}
