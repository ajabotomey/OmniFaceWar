using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hacker Gun", menuName = "Weapons/New Hacker Gun")]
public class HackerGun : HackTypeGun
{
    [Header("Hacker Gun Specific Attributes")]
    [SerializeField] private int abilityChance;

    [Header("Abilities")]
    [SerializeField] private bool tempAlly;
    [SerializeField] private bool goBerserk;
}
