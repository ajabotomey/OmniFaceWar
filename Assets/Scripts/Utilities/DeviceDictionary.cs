using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public static class DeviceDictionary
{
    //// GUID Identifiers for controllers
    //private Guid xboxOneControllerGUID = Guid.Parse("19002688-7406-4f4a-8340-8d25335406c8");
    //private Guid xbox360ControllerGUID = Guid.Parse("d74a350e-fe8b-4e9e-bbcd-efff16d34115");
    //private Guid playstationControllerGUID = Guid.Parse("cd9718bf-a87a-44bc-8716-60a0def28a9f");
    //private Guid nintendoSwitchLeftJoyconGUID = Guid.Parse("3eb01142-da0e-4a86-8ae8-a15c2b1f2a04");
    //private Guid nintendoSwitchRightJoyconGUID = Guid.Parse("605dc720-1b38-473d-a459-67d5857aa6ea");
    //private Guid nintendoSwitchDualJoyconGUID = Guid.Parse("521b808c-0248-4526-bc10-f1d16ee76bf1");
    //private Guid nintendoSwitchHandheldJoyconGUI = Guid.Parse("1fbdd13b-0795-4173-8a95-a2a75de9d204");
    //private Guid nintendoSwitchProController = Guid.Parse("7bf3154b-9db8-4d52-950f-cd0eed8a5819");

    private static Dictionary<DeviceType, System.Guid> deviceDictionary = new Dictionary<DeviceType, System.Guid> {
        { DeviceType.XboxOne, System.Guid.Parse("19002688-7406-4f4a-8340-8d25335406c8") },
        { DeviceType.Xbox360, System.Guid.Parse("d74a350e-fe8b-4e9e-bbcd-efff16d34115") },
        { DeviceType.PS4, System.Guid.Parse("cd9718bf-a87a-44bc-8716-60a0def28a9f") },
    };

    public static DeviceType GetControllerType()
    {
        Player player = ReInput.players.GetPlayer(0); // Assume 0 always

        Controller controller = player.controllers.GetLastActiveController();
        if (controller == null)
            return DeviceType.PC;

        System.Guid controllerGUID = controller.hardwareTypeGuid;

        foreach (KeyValuePair<DeviceType, System.Guid> device in deviceDictionary) {
            if (device.Value == controllerGUID)
                return device.Key;
        }

        return DeviceType.PC;
    }
}
