using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Zenject;

public class UITooltip : MonoBehaviour
{
    public static UITooltip instance;

    [SerializeField] private Text tooltipText;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private float textPaddingSize;
    [SerializeField] private bool paddingBothSides;

    [SerializeField] private Camera uiCamera;

    [Inject] private IInputController inputController;

    void Awake()
    {
        instance = this;
        HideTooltip();
        if (paddingBothSides)
            textPaddingSize *= 2f;
    }

    void Update()
    {
        if (!inputController.IsControllerActive()) {
            Vector2 localPoint;
            RectTransform parentTransform = transform.parent.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform, Mouse.current.position.ReadValue(), uiCamera, out localPoint);
            transform.localPosition = localPoint;
        }
    }

    public void ShowTooltip(string tooltipString)
    {
        if (tooltipString.Equals(""))
            return;

        gameObject.SetActive(true);

        tooltipText.text = tooltipString;

        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize, tooltipText.preferredHeight + textPaddingSize);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public void ShowTooltipController(Vector2 position, string tooltipString)
    {
        transform.localPosition = new Vector2(position.x + 50, position.y + 50);
        ShowTooltip(tooltipString);
    }
}
