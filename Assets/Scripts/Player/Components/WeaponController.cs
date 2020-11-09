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
    INTERNETGUN
}

[System.Serializable]
public class WeaponController
{
    [Header("Weapons")]
    [SerializeField] private Pistol pistol;
    [SerializeField] private SubmachineGun submachineGun;
    [SerializeField] private HackerGun hackerGun;
    [SerializeField] private InternetGun internetGun;

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
        }

        return null;
    }

    public WeaponSelect GetCurrentWeaponType()
    {
        return currentWeapon;
    }
}
