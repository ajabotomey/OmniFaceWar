using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdown : UIObject
{
    [SerializeField] private Dropdown dropdown;
    private string currentValue;

    public void SetOptions(List<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        currentValue = options[0];
    }

    public void SetValue(Dropdown change)
    {
        currentValue = dropdown.options[change.value].text;
    }

    public string GetValue()
    {
        return currentValue;
    }

    public Dropdown GetObject()
    {
        return dropdown;
    }
}
