using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Weapon : WeaponGadget
{
    [Header("Weapon Attributes")]
    [SerializeField] private float energyCapacity;
    [SerializeField] private float energyCost;
    [SerializeField] private bool isCurrentWeapon;
    [SerializeField] private bool isHackTypeGun;
    //[SerializeField] private AudioClip weaponFireSound;

    [Header("Debug")]
    [SerializeField] private float currentEnergy;

    [Header("Event")]
    [SerializeField] private FloatEvent fireEvent;
    [SerializeField] private FloatEvent rechargeEvent;

    public void Initialize()
    {
        currentEnergy = energyCapacity;
    }

    public void Fire()
    {
        currentEnergy -= energyCost;
        fireEvent.Raise(currentEnergy);
        FMODUnity.RuntimeManager.PlayOneShot(WeaponFireSound);
    }

    public void RechargeWeapon()
    {
        currentEnergy += RechargeRate * Time.fixedDeltaTime;

        if (currentEnergy > energyCapacity)
            currentEnergy = energyCapacity;

        // Check if current weapon
        if (isCurrentWeapon)
            rechargeEvent.Raise(currentEnergy);
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public float GetEnergyCapacity()
    {
        return energyCapacity;
    }

    public bool CanWeaponFire()
    {
        return currentEnergy > energyCost;
    }

    public void MakeCurrentWeapon(bool value)
    {
        isCurrentWeapon = value;
    }

    public bool IsHackTypeGun()
    {
        return isHackTypeGun;
    }

    //public AudioClip GetWeaponFireSound()
    //{
    //    return weaponFireSound;
    //}
}
