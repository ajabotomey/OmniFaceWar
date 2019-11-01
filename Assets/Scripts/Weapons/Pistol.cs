using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "Weapons/New Pistol")]
public class Pistol : BulletTypeGun
{
    [Header("Pistol Specific Attributes")]
    [SerializeField] private bool hasSilencer;
    [SerializeField] private bool hasExplosiveRounds;
}
