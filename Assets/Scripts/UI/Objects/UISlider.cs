using UnityEngine;
using UnityEngine.UI;

public class UISlider : UIObject
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text value;
    [SerializeField] private string fieldName;
    [SerializeField] private int minimumValue;
    [SerializeField] private int maximumValue;

    private void Awake()
    {
        if (minimumValue == maximumValue) {
            Logger.Error("Minimum value for " + gameObject.name + " cannot be the same as the maximum value. Please change these values.");
            return;
        }
            
        slider.minValue = minimumValue;
        slider.maxValue = maximumValue;
    }

    // Update is called once per frame
    void Update()
    {
        var sliderValue = Mathf.RoundToInt((slider.value / slider.maxValue) * 100);
        value.text = fieldName + ": " + sliderValue;
    }

    public void SetValue(int value)
    {
        slider.value = value;
    }

    public Slider GetObject()
    {
        return slider;
    }
}
