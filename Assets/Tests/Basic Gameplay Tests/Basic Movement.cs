using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using Zenject;

public class BasicMovement : SceneTestFixture
{
    private Keyboard keyboard;
    private Gamepad gamepad;
    private InputTestFixture input;

    public override void SetUp()
    {
        base.SetUp();

        input = new InputTestFixture();
        keyboard = InputSystem.AddDevice<Keyboard>();
        gamepad = InputSystem.AddDevice<Gamepad>();
    }

    [UnityTest]
    public IEnumerator KeyboardMovement()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerOriginalPosition = player.transform.position;

        // Test the W Key
        input.Press(keyboard.wKey);
        yield return new WaitForSeconds(0.1f);
        input.Release(keyboard.wKey);
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.y, Is.GreaterThan(playerOriginalPosition.y));

        // Reset original position for new test
        playerOriginalPosition = player.transform.position;

        // Test the S Key
        input.Press(keyboard.sKey);
        yield return new WaitForSeconds(0.1f);
        input.Release(keyboard.sKey);
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.y, Is.LessThan(playerOriginalPosition.y));

        playerOriginalPosition = player.transform.position;

        // Test the A Key
        input.Press(keyboard.aKey);
        yield return new WaitForSeconds(0.1f);
        input.Release(keyboard.aKey);
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.x, Is.LessThan(playerOriginalPosition.x));

        playerOriginalPosition = player.transform.position;

        // Test the D Key
        input.Press(keyboard.dKey);
        yield return new WaitForSeconds(0.1f);
        input.Release(keyboard.dKey);
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.x, Is.GreaterThan(playerOriginalPosition.x));
    }

    [UnityTest]
    public IEnumerator GamepadMovement()
    {
        yield return LoadScene("TutorialLevel");
        yield return new WaitForSeconds(2f);

        var player = SceneContainer.Resolve<PlayerControl>();
        var playerOriginalPosition = player.transform.position;

        // Move Up
        input.Set(gamepad.leftStick, new Vector2(0, 1));
        yield return new WaitForSeconds(0.1f);
        input.Set(gamepad.leftStick, new Vector2(0, 0));
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.y, Is.GreaterThan(playerOriginalPosition.y));

        playerOriginalPosition = player.transform.position;

        // Move Down
        input.Set(gamepad.leftStick, new Vector2(0, -1));
        yield return new WaitForSeconds(0.1f);
        input.Set(gamepad.leftStick, new Vector2(0, 0));
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.y, Is.LessThan(playerOriginalPosition.y));

        playerOriginalPosition = player.transform.position;

        // Move Left
        input.Set(gamepad.leftStick, new Vector2(-1, 0));
        yield return new WaitForSeconds(0.1f);
        input.Set(gamepad.leftStick, new Vector2(0, 0));
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.x, Is.LessThan(playerOriginalPosition.x));

        playerOriginalPosition = player.transform.position;

        // Move Right
        input.Set(gamepad.leftStick, new Vector2(1, 0));
        yield return new WaitForSeconds(0.1f);
        input.Set(gamepad.leftStick, new Vector2(0, 0));
        yield return new WaitForSeconds(2f);
        Assert.That(player.transform.position.x, Is.GreaterThan(playerOriginalPosition.x));
    }
}
