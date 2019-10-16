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
    [SerializeField] private Pistol pistol;
    [SerializeField] private SubmachineGun submachineGun;
    [SerializeField] private HackerGun hackerGun;
    [SerializeField] private InternetGun internetGun;

    private WeaponSelect currentWeapon;

    public WeaponController()
    {
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
}
