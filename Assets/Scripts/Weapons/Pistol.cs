using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Pistol", menuName = "Weapons/New Pistol")]
public class Pistol : Weapon
{
    [SerializeField] private int damage;
    [SerializeField] private bool hasSilencer;
    [SerializeField] private bool hasExplosiveRounds;
}
