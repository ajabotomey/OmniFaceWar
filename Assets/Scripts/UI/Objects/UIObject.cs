using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected string tooltipText;

    // Figure out how to trigger this with a controller
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    public void ShowTooltip()
    {
        UITooltip.instance.ShowTooltip(tooltipText);
    }

    public void HideTooltip()
    {
        UITooltip.instance.HideTooltip();
    }

    public void ShowTooltip(Selectable obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector3[] v = new Vector3[4];
        rect.GetWorldCorners(v);

        if (InputController.instance.IsControllerActive())
            UITooltip.instance.ShowTooltipController(v[3], tooltipText); //V[3] is the bottom right corner
    }
}
