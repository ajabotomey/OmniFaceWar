using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

[TestFixture]
public class InputTesting
{
    [Test]
    public void InputTesting_CanUseKeyboardAndMouse()
    {
        var keyboard = InputSystem.AddDevice<Keyboard>();
    }
}
