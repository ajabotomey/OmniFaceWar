using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using Zenject;


public class BasicGunRotation : SceneTestFixture
{
    private Mouse mouse;
    private Gamepad gamepad;
    private InputTestFixture input;
    private Vector3 aim;

    public override void SetUp()
    {
        base.SetUp();

        input = new InputTestFixture();
        mouse = InputSystem.AddDevice<Mouse>();
        gamepad = InputSystem.AddDevice<Gamepad>();
        aim = new Vector3(0.0f, 0.0f, 0.0f);
    }

    #region Mouse Tests

    [UnityTest]
    public IEnumerator AimingWeaponMouseRight()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();

        var rotation = playerInput.actions.FindAction("Rotation");

        // Move Right
        input.Move(mouse.position, new Vector2(200, 0));
        var delta = rotation.ReadValue<Vector2>();
        CalculateMouseAim(delta);
        Assert.That(aim, Is.EqualTo(new Vector3(1.0f, 0.0f, 0.0f)));
    }

    [UnityTest]
    public IEnumerator AimingWeaponMouseLeft()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();

        var rotation = playerInput.actions.FindAction("Rotation");

        // Move Right
        input.Move(mouse.position, new Vector2(-200, 0));
        var delta = rotation.ReadValue<Vector2>();
        CalculateMouseAim(delta);
        Assert.That(aim, Is.EqualTo(new Vector3(-1.0f, 0.0f, 0.0f)));
    }

    [UnityTest]
    public IEnumerator AimingWeaponMouseUp()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();

        var rotation = playerInput.actions.FindAction("Rotation");

        // Move Right
        input.Move(mouse.position, new Vector2(0, 200));
        var delta = rotation.ReadValue<Vector2>();
        CalculateMouseAim(delta);
        Assert.That(aim, Is.EqualTo(new Vector3(0.0f, 1.0f, 0.0f)));
    }

    [UnityTest]
    public IEnumerator AimingWeaponMouseDown()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();

        var rotation = playerInput.actions.FindAction("Rotation");

        // Move Right
        input.Move(mouse.position, new Vector2(0, -200));
        var delta = rotation.ReadValue<Vector2>();
        CalculateMouseAim(delta);
        Assert.That(aim, Is.EqualTo(new Vector3(0.0f, -1.0f, 0.0f)));
    }

    public void CalculateMouseAim(Vector3 delta)
    {
        aim += new Vector3(delta.x, delta.y, 0.0f);
        //Debug.Log("Aim is: " + aim);
        if (aim.magnitude > 1.0f) {
            aim.Normalize();
        }
    }

    #endregion

    [UnityTest]
    public IEnumerator AimingWeaponGamepadRight()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();
        var rotation = playerInput.actions.FindAction("Rotation");

        input.Set(gamepad.rightStick, new Vector2(1, 0));

        aim = rotation.ReadValue<Vector2>();
        Debug.Log(aim);
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(1f, 0f, 0f)));
    }

    [UnityTest]
    public IEnumerator AimingWeaponGamepadLeft()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();
        var rotation = playerInput.actions.FindAction("Rotation");

        input.Set(gamepad.rightStick, new Vector2(-1, 0));

        aim = rotation.ReadValue<Vector2>();
        Debug.Log(aim);
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(-1f, 0f, 0f)));
    }

    [UnityTest]
    public IEnumerator AimingWeaponGamepadUp()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();
        var rotation = playerInput.actions.FindAction("Rotation");

        input.Set(gamepad.rightStick, new Vector2(0, 1));

        aim = rotation.ReadValue<Vector2>();
        Debug.Log(aim);
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(0f, 1f, 0f)));
    }

    [UnityTest]
    public IEnumerator AimingWeaponGamepadDown()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerInput = player.GetPlayerInput();
        var rotation = playerInput.actions.FindAction("Rotation");

        input.Set(gamepad.rightStick, new Vector2(0, -1));

        aim = rotation.ReadValue<Vector2>();
        Debug.Log(aim);
        if (aim.magnitude > 1.0f)
            aim.Normalize();

        Assert.That(aim, Is.EqualTo(new Vector3(0f, -1f, 0f)));
    }

}
