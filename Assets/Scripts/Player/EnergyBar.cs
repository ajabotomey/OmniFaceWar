using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private RectTransform image;

    [Inject] private WeaponController weaponControl;

    float maxWidth;
    float height;
    float energyCapacity;

    public void Initialize(float _energyCapacity)
    {
        maxWidth = image.localScale.x;
        height = image.localScale.y;
        energyCapacity = _energyCapacity;
    }

    public void ChangeWeapon(float currentEnergy, float energyCapacity)
    {
        this.energyCapacity = energyCapacity;

        if (currentEnergy != energyCapacity) {
            // Modify the bar to reflect this
        }
    }

    public void UpdateWeaponEnergy(float currentEnergy)
    {
        if (weaponControl.GetCurrentWeapon() == null)
            return;

        float percent = currentEnergy / energyCapacity;
        var width = percent * maxWidth;
        image.localScale = new Vector3(width, height, 1);
    }
}
