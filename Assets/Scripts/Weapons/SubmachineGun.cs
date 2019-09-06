using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Submachine Gun", menuName = "Weapons/New Submachine Gun")]
public class SubmachineGun : Weapon
{
    [Header("Gun Specific Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private bool incendiaryRounds;
}
