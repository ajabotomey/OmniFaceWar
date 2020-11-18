﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public interface IInputController
{
    bool IsControllerActive();
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
    float GetUIHorizontal();
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
    void SetInputDelay(bool enabled);
    void SetInputSensitivity(int value);
    void SetRumbleSensitivity(int value);
    bool ToggleObjectivesPanel();
    ActionElementMap GetActionElementMap(int actionID);
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

    private float inputDelay = 0.5f;

    private const float JOYSTICK_SENSITIVITY = 1;
    private const float MOUSE_SENSITIVITY = 1;
    private const float KEYBOARD_SENSITIVITY = 3;

    private float rumbleSensitivity = 100f;

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

        if (!joystick.supportsVibration) return;
        for (int i = 0; i < joystick.vibrationMotorCount; i++) {
            joystick.SetVibration(i, rumbleSensitivity, duration);
        }
    }

    public void StopRumble()
    {
        joystick.StopVibration();
    }

    public float GetUIHorizontal()
    {
        return player.GetAxis("UIHorizontal");
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

    public void SetInputDelay(bool enabled)
    {
        if (enabled)
            player.controllers.maps.GetInputBehavior(0).buttonRepeatDelay = inputDelay;
        else
            player.controllers.maps.GetInputBehavior(0).buttonRepeatDelay = 0;
    }

    public bool OpenNotificationWindow()
    {
        return player.GetButtonDown("OpenNotificationWindow");
    }

    private void OnControllerConnected(ControllerStatusChangedEventArgs args) // Need to plug into this to update the UI
    {
        player.controllers.Keyboard.enabled = false;
        player.controllers.Mouse.enabled = false;
    }

    private void OnControllerDisconnected(ControllerStatusChangedEventArgs args) // Need to plug into this to update the UI
    {
        player.controllers.Keyboard.enabled = true;
        player.controllers.Mouse.enabled = true;
    }

    public void SetInputSensitivity(int value)
    {
        InputBehavior behavior = player.controllers.maps.GetInputBehavior(0);

        behavior.joystickAxisSensitivity = JOYSTICK_SENSITIVITY * (value / 5); // Standard is 5, Maximum is 10
        behavior.mouseXYAxisSensitivity = MOUSE_SENSITIVITY * (value / 5);
        behavior.digitalAxisSensitivity = KEYBOARD_SENSITIVITY * (value / 5);
    }

    public void SetRumbleSensitivity(int value)
    {
        rumbleSensitivity = value * 10;
    }

    public bool ToggleObjectivesPanel()
    {
        return player.GetButtonDown("ToggleObjectivesPanel");
    }

    public ActionElementMap GetActionElementMap(int actionID)
    {
        if (player.controllers.Keyboard.enabled)
        {
            ActionElementMap aem = player.controllers.maps.GetFirstElementMapWithAction(player.controllers.Keyboard, actionID, true);
            return aem;
        } else if (joystick != null)
        {
            ActionElementMap aem = player.controllers.maps.GetFirstElementMapWithAction(joystick, actionID, true);
            return aem;
        }

        return null;
    }
}
