using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class KeyboardTesting : InputTestFixture
{
    [Test]
    public void KeyboardMovement()
    {
        var keyboard = InputSystem.AddDevice<Keyboard>();

        var movement = new InputAction("Movement", InputActionType.Value);
        movement.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard/s")
            .With("Left", "<Keyboard/a")
            .With("Right", "<Keyboard/d");

        movement.Enable();

        // Test W Key (up)
        Press(keyboard.wKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, 1)));
        Release(keyboard.wKey);

        // Test S Key (down)
        Press(keyboard.sKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, -1)));
        Release(keyboard.sKey);

        // Test A Key (left)
        Press(keyboard.aKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(-1, 0)));
        Release(keyboard.aKey);

        // Test D Key (right)
        Press(keyboard.dKey);
        Assert.That(movement.ReadValue<Vector2>(), Is.EqualTo(new Vector2(1, 0)));
        Release(keyboard.dKey);
    }
}
