using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIToggle : UIObject
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Text value;
    [SerializeField] private string fieldName;

    void Awake()
    {
        value.text = fieldName;
    }

    public void SetValue(bool value)
    {
        toggle.isOn = value;
    }

    public Toggle GetObject()
    {
        return toggle;
    }
}
