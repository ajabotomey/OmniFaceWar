using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;

public enum WeaponSelect
{
    NONE, // No weapon in the base
    PISTOL,
    SUBMACHINEGUN,
    HACKERGUN,
    INTERNETGUN,
    NOISEMAKER,
    SMOKEBOMB,
    VIRALBOMB,
    SMARTFACEMASK
}

[System.Serializable]
public class WeaponController
{
    [Header("Weapons")]
    [SerializeField] private Pistol pistol;
    [SerializeField] private SubmachineGun submachineGun;
    [SerializeField] private HackerGun hackerGun;
    [SerializeField] private InternetGun internetGun;
    [SerializeField] private Noisemaker noisemaker;
    [SerializeField] private Smokebomb smokebomb;
    [SerializeField] private ViralBomb viralBomb;
    [SerializeField] private SmartFaceMask smartFaceMask;

    [Header("Events")]
    [SerializeField] private FloatEvent initializeEvent;

    private WeaponSelect currentWeapon;

    public void Initialize()
    {
        currentWeapon = WeaponSelect.NONE;

        pistol.Initialize();
        submachineGun.Initialize();
        hackerGun.Initialize();
        internetGun.Initialize();
    }

    public void RechargeGuns()
    {
        pistol.RechargeWeapon();
        submachineGun.RechargeWeapon();
        hackerGun.RechargeWeapon();
        internetGun.RechargeWeapon();
    }

    public void SelectPistol()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.PISTOL;
        pistol.Select();
        // TODO: Make other weapons false
        initializeEvent.Raise(pistol.GetEnergyCapacity());
    }

    public void SelectMachineGun()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.SUBMACHINEGUN;
        submachineGun.Select();
        initializeEvent.Raise(submachineGun.GetEnergyCapacity());
    }

    public void SelectHackerGun()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.HACKERGUN;
        hackerGun.Select();
        initializeEvent.Raise(hackerGun.GetEnergyCapacity());
    }

    public void SelectInternetGun()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.INTERNETGUN;
        internetGun.Select();
        initializeEvent.Raise(internetGun.GetEnergyCapacity());
    }

    public void SelectNoisemaker()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.NOISEMAKER;
        noisemaker.Select();
    }

    public void SelectSmokebomb()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.SMOKEBOMB;
        smokebomb.Select();
    }

    public void SelectViralBomb()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.VIRALBOMB;
        viralBomb.Select();
    }

    public void SelectSmartFaceMask()
    {
        DeselectWeaponGadgets();
        currentWeapon = WeaponSelect.SMARTFACEMASK;
        smartFaceMask.Select();
    }

    private void DeselectWeaponGadgets()
    {
        pistol.Deselect();
        submachineGun.Deselect();
        hackerGun.Deselect();
        internetGun.Deselect();
        noisemaker.Deselect();
        smokebomb.Deselect();
        viralBomb.Deselect();
        smartFaceMask.Deselect();
    }

    public WeaponSelect GetCurrentWeaponSelect()
    {
        return currentWeapon;
    }

    public Weapon GetCurrentWeapon()
    {
        switch(currentWeapon) {
            case WeaponSelect.PISTOL:
                return pistol;
            case WeaponSelect.SUBMACHINEGUN:
                return submachineGun;
            case WeaponSelect.HACKERGUN:
                return hackerGun;
            case WeaponSelect.INTERNETGUN:
                return internetGun;
            default:
                return null;
        }
    }

    public Gadget GetCurrentGadget()
    {
        switch(currentWeapon) {
            case WeaponSelect.NOISEMAKER:
                return noisemaker;
            case WeaponSelect.SMOKEBOMB:
                return smokebomb;
            case WeaponSelect.VIRALBOMB:
                return viralBomb;
            case WeaponSelect.SMARTFACEMASK:
                return smartFaceMask;
            default:
                return null;
        }
    }

    public WeaponSelect GetCurrentWeaponType()
    {
        return currentWeapon;
    }
}
