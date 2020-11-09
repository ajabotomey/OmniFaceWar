using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShowControllerButtonUI : MonoBehaviour
{
    // TODO: Rewrite to take into account rebinding 
    [SerializeField] private ControllerActionButton action;
    [SerializeField] private Image image;

    // Update is called once per frame
    void Update()
    {
        DeviceType deviceType = DeviceDictionary.GetControllerType();

        switch (deviceType) {
            case DeviceType.PC:
                image.sprite = action.pcImage;
                break;
            case DeviceType.XboxOne:
            case DeviceType.Xbox360:
                image.sprite = action.xboxImage;
                break;
            case DeviceType.PS4:
                image.sprite = action.playstationImage;
                break;
        }
    }
}
