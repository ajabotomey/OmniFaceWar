using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Weapon : ScriptableObject
{
    [Header("Base Attributes")]
    [SerializeField] private float fireRate;
    [SerializeField] private float energyCapacity;
    [SerializeField] private float energyCost;
    [SerializeField] private float rechargeRate;
    [SerializeField] private bool isCurrentWeapon;
    [SerializeField] private bool isHackTypeGun;
    //[SerializeField] private AudioClip weaponFireSound;
    [FMODUnity.EventRef] [SerializeField] private string weaponFireSound;

    [Header("Debug")]
    [SerializeField] private float currentEnergy;

    [Header("Event")]
    [SerializeField] private FloatEvent fireEvent;
    [SerializeField] private FloatEvent rechargeEvent;

    public void Initialize()
    {
        currentEnergy = energyCapacity;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public void Fire()
    {
        currentEnergy -= energyCost;
        fireEvent.Raise(currentEnergy);
        FMODUnity.RuntimeManager.PlayOneShot(weaponFireSound);
    }

    public void RechargeWeapon()
    {
        currentEnergy += rechargeRate * Time.fixedDeltaTime;

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
