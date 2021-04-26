using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class GamepadTesting : InputTestFixture
{
    [Test]
    public void GamepadMovement()
    {
        var gamepad = InputSystem.AddDevice<Gamepad>();
        var action = new InputAction("Movement", InputActionType.Value, "<Gamepad>/leftStick");
        action.Enable();

        // Move Up
        Set(gamepad.leftStick, new Vector2(0, 1));
        Assert.That(action.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, 1)));
        RestartAction(action);

        // Move Down
        Set(gamepad.leftStick, new Vector2(0, -1));
        Assert.That(action.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, -1)));
        RestartAction(action);

        // Move Left
        Set(gamepad.leftStick, new Vector2(-1, 0));
        Assert.That(action.ReadValue<Vector2>(), Is.EqualTo(new Vector2(-1, 0)));
        RestartAction(action);

        // Move Right
        Set(gamepad.leftStick, new Vector2(1, 0));
        Assert.That(action.ReadValue<Vector2>(), Is.EqualTo(new Vector2(1, 0)));
        action.Dispose();
    }

    [Test]
    public void GamepadFireWeapon()
    {
        var gamepad = InputSystem.AddDevice<Gamepad>();

        var action = new InputAction("Fire Weapon", InputActionType.Button);
        action.AddBinding("<Gamepad>/leftTrigger");

        action.Enable();

        Press(gamepad.leftTrigger);

        Assert.That(action.triggered, Is.EqualTo(true));
    }

    [Test]
    public void GamepadAimWeaponRight()
    {
        var gamepad = InputSystem.AddDevice<Gamepad>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Gamepad>/rightStick");
        rotation.Enable();

        Set(gamepad.rightStick, new Vector2(1, 0));

        aim = rotation.ReadValue<Vector2>();
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(1.0f, 0.0f, 0.0f)));
    }

    [Test]
    public void GamepadAimWeaponLeft()
    {
        var gamepad = InputSystem.AddDevice<Gamepad>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Gamepad>/rightStick");
        rotation.Enable();

        Set(gamepad.rightStick, new Vector2(-1, 0));

        aim = rotation.ReadValue<Vector2>();
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(-1.0f, 0.0f, 0.0f)));
    }

    [Test]
    public void GamepadAimWeaponUp()
    {
        var gamepad = InputSystem.AddDevice<Gamepad>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Gamepad>/rightStick");
        rotation.Enable();

        Set(gamepad.rightStick, new Vector2(0, 1));

        aim = rotation.ReadValue<Vector2>();
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(0.0f, 1.0f, 0.0f)));
    }

    [Test]
    public void GamepadAimWeaponDown()
    {
        var gamepad = InputSystem.AddDevice<Gamepad>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Gamepad>/rightStick");
        rotation.Enable();

        Set(gamepad.rightStick, new Vector2(0, -1));

        aim = rotation.ReadValue<Vector2>();
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(0.0f, -1.0f, 0.0f)));
    }

    public void RestartAction(InputAction action)
    {
        action.Disable();
        action.Enable();
    }
}
