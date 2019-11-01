using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Submachine Gun", menuName = "Weapons/New Submachine Gun")]
public class SubmachineGun : BulletTypeGun
{
    [Header("SMG Specific Attributes")]
    [SerializeField] private bool incendiaryRounds;
}
