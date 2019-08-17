using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;

public interface IWeaponController
{
    void SelectWeapon(int index);
    void SelectGun();
    void SelectNoise();
    void SelectMask();
}

public class WeaponController : MonoBehaviour, IWeaponController
{
    [SerializeField] private Texture2D noisemaker;
    [SerializeField] private Texture2D smokebomb;

    public enum WeaponState { GUN, GRENADE, NOISE, MASK }
    public WeaponState CurrentWeapon { get; set; }

    public int WeaponCount { get; set; }

    private IInputController _inputController;

    [Inject]
    public void Construct(IInputController inputController)
    {
        _inputController = inputController;
    }

    // Start is called before the first frame update
    void Start()
    {
        WeaponCount = Enum.GetValues(typeof(WeaponState)).Length;
    }

    // Update is called once per frame
    void Update()
    {
        // Keyboard input
        if (_inputController.SelectGun()) {
            SelectGun();
        }  else if (_inputController.SelectNoise()) {
            SelectNoise();
        } else if (_inputController.SelectMask()) {
            SelectMask();
        }

        // Mouse scrollwheel input
        if (_inputController.ScrollWeapons() > 0f) {
            if (CurrentWeapon == WeaponState.GUN)
                CurrentWeapon = (WeaponState)(WeaponCount - 1);
            else
                CurrentWeapon--;

            SelectWeapon((int)CurrentWeapon);
        } else if (_inputController.ScrollWeapons() < 0f) {
            int value = (int)CurrentWeapon;
            if (value == WeaponCount - 1)
                CurrentWeapon = WeaponState.GUN;
            else
                CurrentWeapon++;

            SelectWeapon((int)CurrentWeapon);
        }

        // TODO: Fix once UI Controller is available
        //if (!UIController.instance.isInMenu) {
        //    if (InputController.instance.SelectWeapon()) {
        //        UIController.instance.ShowWeaponWheel();
        //    } else {
        //        UIController.instance.HideWeaponWheel();
        //    }
        //}
    }

    public void SelectWeapon(int index)
    {
        if (index == 0) {
            SelectGun();
        } else if (index == 1) {
            Logger.Debug("Selected Grenade");
        } else if (index == 2) {
            SelectNoise();
        } else if (index == 3) {
            SelectMask();
        }
    }

    public void SelectGun()
    {
        Logger.Debug("Return to Gun");
        Cursor.visible = false;
        CurrentWeapon = WeaponState.GUN;
    }

    public void SelectNoise()
    {
        Cursor.visible = true;
        Logger.Debug("Swap to Noisemaker");
        Cursor.SetCursor(noisemaker, new Vector3(0, 0, -1), CursorMode.Auto);
        CurrentWeapon = WeaponState.NOISE;
    }

    public void SelectMask()
    {
        Logger.Debug("Put on / take off SmartMask");
        Cursor.visible = false;
        CurrentWeapon = WeaponState.MASK;
    }
}
