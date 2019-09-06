using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "Weapons/New Pistol")]
public class Pistol : Weapon
{
    [Header("Gun Specific Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private bool hasSilencer;
    [SerializeField] private bool hasExplosiveRounds;
}
