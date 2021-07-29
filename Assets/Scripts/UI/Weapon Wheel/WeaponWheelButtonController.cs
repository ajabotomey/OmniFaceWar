using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private int ID;
    [SerializeField] private string itemName;
    [SerializeField] TextMeshProUGUI itemText;
    [SerializeField] private Image selectedItem;
    [SerializeField] private Sprite icon;
    [SerializeField] private Animator anim;

    private bool selected = false;

    // Update is called once per frame
    void Update()
    {
        if (selected) {
            selectedItem.sprite = icon;
            itemText.text = itemName;
        }
    }

    public int GetID()
    {
        return ID;
    }

    public void OnSelect(BaseEventData eventData) {
        Selected();
    }

    public void ButtonSelect()
    {
        EventSystemManager.Instance.SetCurrentSelectedGameObject(this.gameObject);
    }

    private void Selected()
    {
        selected = true;
        WeaponWheelController.weaponID = ID;
        //EventSystemManager.Instance.SetCurrentSelectedGameObject(this.gameObject);
    }

    public void OnDeselect(BaseEventData eventData) {
        Deselected();
    }

    public void Deselected()
    {
        selected = false;
        WeaponWheelController.weaponID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", true);
        itemText.text = itemName;
    }

    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }
}
