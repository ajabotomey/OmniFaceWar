using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : Selectable, IPointerClickHandler
{
    [SerializeField] private UnityEvent OnClick;
    [SerializeField] private Image image;
    [SerializeField] private Material glowMaterial;
    [SerializeField] private VoidEvent selectButton;

    public override void Select()
    {
        base.Select();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        image.material = glowMaterial;
        if (selectButton)
            selectButton.Raise();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        image.material = null;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        image.material = glowMaterial;
        //selectButton.Raise();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        image.material = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
    }
}
