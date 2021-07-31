using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Zenject;

public class WeaponWheelController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private WeaponWheelButtonController[] buttons; // Buttons are in clockwise

    [Header("Other Attributes")]
    [SerializeField] private Animator anim;
    [SerializeField] private Image selectedItem;
    [SerializeField] private Sprite noImage;
    [SerializeField] private GameObject defaultSelectedButton;
    public static int weaponID = 0;

    [Inject] private WeaponController _weaponControl;

    public void HandleWeaponWheel(bool weaponWheelSelected)
    {
        if (weaponWheelSelected) {
            anim.SetTrigger("TriggerWheel");

            if (weaponID == 0) {
                EventSystemManager.Instance.SetCurrentSelectedGameObject(defaultSelectedButton);
            } else {
                foreach (WeaponWheelButtonController button in buttons) {
                    if (button.GetID() == weaponID) {
                            EventSystemManager.Instance.SetCurrentSelectedGameObject(button.gameObject);
                    }
                }
            }
        } else {
            anim.SetTrigger("TriggerWheel");
        }

        SelectWeapon();
    }

    public void SelectWeapon()
    {
        switch (weaponID) {
            case 0:
                selectedItem.sprite = noImage;
                break;
            case 1:
                _weaponControl.SelectPistol();
                break;
            case 2:
                _weaponControl.SelectMachineGun();
                break;
            case 3:
                _weaponControl.SelectNoisemaker();
                break;
            case 4:
                _weaponControl.SelectInternetGun();
                break;
            case 5:
                _weaponControl.SelectHackerGun();
                break;
            case 6:
                _weaponControl.SelectViralBomb();
                break;
            case 7:
                _weaponControl.SelectSmokebomb();
                break;
            case 8:
                _weaponControl.SelectSmartFaceMask();
                break;
        }
    }

    public void SelectButton()
    {
        // Deselect all buttons first
        foreach (WeaponWheelButtonController button in buttons) {
            button.Deselected();
        }
    }

    public void HideWeaponWheel()
    {
        gameObject.SetActive(false);
    }

    public void ControllerSelect(Vector2 input)
    {
        // Define vectors
        Vector2 top = new Vector2(0, 1);
        Vector2 topRight = new Vector2(1, 1);
        Vector2 right = new Vector2(1, 0);
        Vector2 bottomRight = new Vector2(1, -1);
        Vector2 bottom = new Vector2(0, -1);
        Vector2 bottomLeft = new Vector2(-1, -1);
        Vector2 left = new Vector2(-1, 0);
        Vector2 topLeft = new Vector2(-1, 1);

        if (input == top) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[0].gameObject);
        } else if (input == topRight) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[1].gameObject);
        } else if (input == right) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[2].gameObject);
        } else if (input == bottomRight) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[3].gameObject);
        } else if (input == bottom) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[4].gameObject);
        } else if (input == bottomLeft) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[5].gameObject);
        } else if (input == left) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[6].gameObject);
        } else if (input == topLeft) {
            EventSystemManager.Instance.SetCurrentSelectedGameObject(buttons[7].gameObject);
        }
    }
}
