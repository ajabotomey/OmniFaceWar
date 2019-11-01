using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Rewired;

[CreateAssetMenu(fileName = "Rewired Action Manager", menuName = "Rewired Action Manager")]
public class RewiredActionManager : ScriptableObject
{
    [SerializeField] private List<ControllerActionButton> actionList;

    public Sprite GetSpriteFromAction(int actionID)
    {
        var action = actionList.First(i => i.rewiredAction == actionID);

        DeviceType deviceType = DeviceDictionary.GetControllerType();

        switch (deviceType) {
            case DeviceType.PC:
                return action.pcImage;
            case DeviceType.XboxOne:
            case DeviceType.Xbox360:
                return action.xboxImage;
            case DeviceType.PS4:
                return action.playstationImage;
        }

        return null;
    }
}
