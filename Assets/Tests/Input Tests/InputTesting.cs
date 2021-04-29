using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
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



    //[Test]
    //public void InputTesting_MouseRotation()
    //{
    //    var mouse = InputSystem.AddDevice<Mouse>();

    //    var rotation = new InputAction("Rotation", InputActionType.Value);
    //    rotation.AddBinding("<Mouse>/delta");

    //    rotation.Enable();

    //    // Move directly right
    //    Move(mouse.position, new Vector2(1, 0));
    //    Assert.That(rotation.ReadValue<Vector2>(), Is.EqualTo(new Vector2(1, 0)));
    //    InputSystem.Update();

    //    // Move directly left
    //    Move(mouse.position, new Vector2(-1, 0));
    //    Assert.That(rotation.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, 0)));
    //    //Move(mouse.position, new Vector2(1, 0));

    //    //// Move directly up
    //    //Move(mouse.position, new Vector2(0, 1));
    //    //Assert.That(rotation.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, 1)));
    //    //Move(mouse.position, new Vector2(0, -1));

    //    //// Move directly down
    //    //Move(mouse.position, new Vector2(0, -1));
    //    //Assert.That(rotation.ReadValue<Vector2>(), Is.EqualTo(new Vector2(0, -1)));
    //    //Move(mouse.position, new Vector2(0, 1));
    //}

    //[Test]
    //public void ControllerImageInput()
    //{
    //    var gameObject = new GameObject();
    //    var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    //    var deviceDisplay = (DeviceDisplayConfigurator)AssetDatabase.LoadAssetAtPath("", typeof(DeviceDisplayConfigurator));

    //}


}
