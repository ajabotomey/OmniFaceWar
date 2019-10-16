using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    [Header("Base Attributes")]
    [SerializeField] private int fireRate;
    [SerializeField] private float energyCapacity;
    [SerializeField] private float energyCost;
    [SerializeField] private float rechargeRate;

    [Header("Debug")]
    [SerializeField] private float currentEnergy;

    public void Initialize()
    {
        currentEnergy = energyCapacity;
    }

    public int GetFireRate()
    {
        return fireRate;
    }

    public void Fire()
    {
        currentEnergy -= energyCost;
    }

    public void RechargeWeapon()
    {
        currentEnergy += (rechargeRate * Time.fixedDeltaTime);

        if (currentEnergy > energyCapacity)
            currentEnergy = energyCapacity;
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public bool CanWeaponFire()
    {
        return currentEnergy > energyCost;
    }
}
