using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public interface IInputController
{
    bool IsControllerActive();
    Vector2 MousePosition();
    float Horizontal();
    float Vertical();
    float Rotation();
    bool FireWeapon();
    bool SmokeBomb();
    bool SelectWeapon();
    void SetRumble(float duration);
    void StopRumble();
    bool UICancel();
    bool Pause();
    bool TakeScreenshot();
}

public class InputController : IInputController
{
    private int playerID = 0;
    private Player player;

    // Controller Maps
    private ControllerMap mouseMap;
    private ControllerMap keyboardMap;
    private ControllerMap joystickMap;
    private Mouse mouse;
    private Joystick joystick;

    public InputController()
    {
        player = ReInput.players.GetPlayer(playerID);

        if (player.controllers.Mouse.isConnected)
            mouse = ReInput.controllers.Mouse;

        // Check which input should be used.
        if (player.controllers.Joysticks.Count == 1) {
            joystick = player.controllers.Joysticks[0]; // Only ever be one joystick
        } else {
            joystick = null;
        }
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

    public float Rotation()
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
}
