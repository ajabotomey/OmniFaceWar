using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIToggle : UIObject
{
    [SerializeField] private Toggle toggle;

    public void SetValue(bool value)
    {
        toggle.isOn = value;
    }

    public Toggle GetObject()
    {
        return toggle;
    }
}
