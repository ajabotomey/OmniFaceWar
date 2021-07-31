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
        currentWeapon = WeaponSelect.PISTOL;
        pistol.MakeCurrentWeapon(true);
        // TODO: Make other weapons false
        initializeEvent.Raise(pistol.GetEnergyCapacity());
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

    public void SetCurrentWeapon()
    {
        switch(currentWeapon) {
            case WeaponSelect.PISTOL:
                SelectPistol();
                break;
        }
    }

    public WeaponSelect GetCurrentWeaponType()
    {
        return currentWeapon;
    }
}
