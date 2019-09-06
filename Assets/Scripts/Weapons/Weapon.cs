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

    public int GetFireRate()
    {
        return fireRate;
    }
}
