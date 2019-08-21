using UnityEngine;
using UnityEngine.UI;

public class UISlider : UIObject
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text value;
    [SerializeField] private string fieldName;
    [SerializeField] private int minimumValue;
    [SerializeField] private int maximumValue;

    private bool initialized = false;
    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (minimumValue == maximumValue) {
            Logger.Error("Minimum value for " + gameObject.name + " cannot be the same as the maximum value. Please change these values.");
            return;
        }

        slider.minValue = minimumValue;
        slider.maxValue = maximumValue;

        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        value.text = fieldName + ": " + slider.value;
    }

    public void SetValue(int value)
    {
        if (!initialized)
            Initialize();

        slider.value = value;
    }

    public Slider GetObject()
    {
        return slider;
    }
}
