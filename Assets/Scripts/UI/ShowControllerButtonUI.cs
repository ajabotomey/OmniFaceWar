//using UnityEngine;
//using UnityEngine.UI;
//using Zenject;
//using Rewired;

//public class ShowControllerButtonUI : MonoBehaviour
//{
//    [ActionIdProperty(typeof(RewiredConsts.Action))]
//    public int rewiredAction;
//    [SerializeField] private Image spriteRenderer;

//    [Inject] private ControllerMap controlMap;
//    [Inject] private IInputController input;

//    private void Start()
//    {
//        UpdateImage();
//    }

//    // Update is called once per frame
//    //void Update()
//    //{

//    //}

//    public void SetAction(int actionID)
//    {
//        rewiredAction = actionID;
//    }

//    public void UpdateImage()
//    {
//        if (rewiredAction == -1)
//            return;

//        DeviceType deviceType = DeviceDictionary.GetControllerType();
//        ActionElementMap aem = input.GetActionElementMap(rewiredAction);

//        switch (deviceType)
//        {
//            case DeviceType.PC:
//                spriteRenderer.sprite = controlMap.GetKeyboardSprite(aem.elementIdentifierId);
//                break;
//            case DeviceType.XboxOne:
//                spriteRenderer.sprite = controlMap.GetControllerGlyph(DeviceDictionary.GetGuid(DeviceType.XboxOne), aem.elementIdentifierId, AxisRange.Full);
//                break;
//            case DeviceType.Xbox360:
//                spriteRenderer.sprite = controlMap.GetControllerGlyph(DeviceDictionary.GetGuid(DeviceType.Xbox360), aem.elementIdentifierId, AxisRange.Full);
//                break;
//            case DeviceType.PS4:
//                spriteRenderer.sprite = controlMap.GetControllerGlyph(DeviceDictionary.GetGuid(DeviceType.PS4), aem.elementIdentifierId, AxisRange.Full);
//                break;
//        }
//    }
//}
