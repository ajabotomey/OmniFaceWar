using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

public class InputTesting : InputTestFixture
{
    //private Keyboard keyboard;
    //private Mouse mouse;
    //private Gamepad gamepad;

    //[OneTimeSetUp]
    //public void InitializeTestEnvironment()
    //{
    //    // Load our scene
    //    //SceneManager.LoadScene("TutorialLevel");

    //    // Attach our devices
    //    keyboard = InputSystem.AddDevice<Keyboard>();
    //    mouse = InputSystem.AddDevice<Mouse>();
    //    gamepad = InputSystem.AddDevice<Gamepad>();

    //    // Wait for enough time to load the scene
    //    //yield return new WaitForSeconds(10);
    //}

    [Test]
    public void InputTesting_KeyboardMovement()
    {
        var keyboard = InputSystem.AddDevice<Keyboard>();

        var movement = new InputAction("Movement", InputActionType.Value);
        movement.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard/s")
            .With("Left", "<Keyboard/a")
            .With("Right", "<Keyboard/d");

        movement.Enable();

        // Test W Key
        Press(keyboard.wKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, 1)));
        Release(keyboard.wKey);

        // Test S Key
        Press(keyboard.sKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, -1)));
        Release(keyboard.sKey);

        // Test A Key
        Press(keyboard.aKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(-1, 0)));
        Release(keyboard.aKey);

        // Test D Key
        Press(keyboard.dKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(1, 0)));
        Release(keyboard.dKey);
    }
}
