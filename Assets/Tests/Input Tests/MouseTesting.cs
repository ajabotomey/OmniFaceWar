using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class MouseTesting : InputTestFixture
{
    [Test]
    public void AimWeaponRight()
    {
        var mouse = InputSystem.AddDevice<Mouse>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Mouse>/delta");
        rotation.Enable();

        Move(mouse.position, new Vector2(20, 0));

        var delta = rotation.ReadValue<Vector2>();

        aim += new Vector3(delta.x, delta.y, 0.0f);
        //Debug.Log("Aim is: " + aim);
        if (aim.magnitude > 1.0f) {
            aim.Normalize();
        }

        Assert.That(aim, Is.EqualTo(new Vector3(1.0f, 0.0f, 0.0f)));
    }

    [Test]
    public void AimWeaponLeft()
    {
        var mouse = InputSystem.AddDevice<Mouse>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Mouse>/delta");
        rotation.Enable();

        Move(mouse.position, new Vector2(-20, 0));

        var delta = rotation.ReadValue<Vector2>();

        aim += new Vector3(delta.x, delta.y, 0.0f);
        //Debug.Log("Aim is: " + aim);
        if (aim.magnitude > 1.0f) {
            aim.Normalize();
        }

        Assert.That(aim, Is.EqualTo(new Vector3(-1.0f, 0.0f, 0.0f)));
    }

    [Test]
    public void AimWeaponUp()
    {
        var mouse = InputSystem.AddDevice<Mouse>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Mouse>/delta");
        rotation.Enable();

        Move(mouse.position, new Vector2(0, 20));

        var delta = rotation.ReadValue<Vector2>();

        aim += new Vector3(delta.x, delta.y, 0.0f);
        //Debug.Log("Aim is: " + aim);
        if (aim.magnitude > 1.0f) {
            aim.Normalize();
        }

        Assert.That(aim, Is.EqualTo(new Vector3(0.0f, 1.0f, 0.0f)));
    }

    [Test]
    public void AimWeaponDown()
    {
        var mouse = InputSystem.AddDevice<Mouse>();
        var aim = new Vector3(0.0f, 0.0f, 0.0f);

        var rotation = new InputAction("Rotation", InputActionType.Value);
        rotation.AddBinding("<Mouse>/delta");
        rotation.Enable();

        Move(mouse.position, new Vector2(0, -20));

        var delta = rotation.ReadValue<Vector2>();

        aim += new Vector3(delta.x, delta.y, 0.0f);
        //Debug.Log("Aim is: " + aim);
        if (aim.magnitude > 1.0f) {
            aim.Normalize();
        }

        Assert.That(aim, Is.EqualTo(new Vector3(0.0f, -1.0f, 0.0f)));
    }

    [Test]
    public void FireWeaponViaMouse()
    {
        var mouse = InputSystem.AddDevice<Mouse>();

        var action = new InputAction("Fire Weapon", InputActionType.Button);
        action.AddBinding("<Mouse>/leftButton");

        action.Enable();

        Click(mouse.leftButton);

        Assert.That(action.triggered, Is.EqualTo(true));
    }
}
