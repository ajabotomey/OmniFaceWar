using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(Selectable))]
public class UIOptionCarousel : UIObject, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Text optionLabel;

    [SerializeField] private Text value;
    [SerializeField] private string fieldName;

    [SerializeField] private UnityEvent OnValueChanged;

    private List<string> options = new List<string>();
    private int currentIndex = 0;
    private bool selected = false;
    private bool midSelection = false;
    private float inputLag = 0.3f;

    [Inject] private IInputController input;

    void Awake()
    {
        value.text = fieldName;
        UpdateLabel();
    }

    void Update()
    {
        if (selected) {
            float horizontal = input.GetUIHorizontal();

            if (horizontal < 0 && !midSelection) {
                //LeftClicked();
                StartCoroutine(LeftClickedController());
            } else if (horizontal > 0 && !midSelection) {
                //RightClicked();
                StartCoroutine(RightClickedController());
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
    }

    public void SetOptions(string[] _options)
    {
        foreach (string option in _options) {
            options.Add(option);
        }
    }

    public void SetOptions(List<string> _options)
    {
        foreach (string option in _options) {
            options.Add(option);
        }
    }

    public void SetCurrentOption(string currentOption)
    {
        if (options.Contains(currentOption)) {
            currentIndex = options.IndexOf(currentOption);

            UpdateLabel();
        }
    }

    public void SetCurrentOption(int index)
    {
        currentIndex = index;
    }

    public string GetCurrentValue()
    {
        return options[currentIndex];
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
    
    public Selectable GetObject()
    {
        return GetComponent<Selectable>();
    }

    private void UpdateLabel()
    {
        // Update label accordingly
        optionLabel.text = options[currentIndex];
    }

    public void LeftClicked()
    {
        if (currentIndex == 0) {
            currentIndex = options.Count - 1;
        } else {
            currentIndex--;
        }

        UpdateLabel();
        OnValueChanged.Invoke();
    }

    public void RightClicked()
    {
        if (currentIndex == options.Count - 1) {
            currentIndex = 0;
        } else {
            currentIndex++;
        }

        UpdateLabel();
        OnValueChanged.Invoke();
    }

    private IEnumerator LeftClickedController()
    {
        midSelection = true;
        LeftClicked();
        yield return new WaitForSecondsRealtime(inputLag);
        midSelection = false;
    }
    
    private IEnumerator RightClickedController()
    {
        midSelection = true;
        RightClicked();
        yield return new WaitForSecondsRealtime(inputLag);
        midSelection = false;
    }
}
