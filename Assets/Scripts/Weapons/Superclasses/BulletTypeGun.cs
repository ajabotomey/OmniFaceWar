using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTypeGun : Weapon
{
    [Header("Bullet Type Attributes")]
    [SerializeField] private int damage;

    public int Damage {
        get { return damage; }
    }
}
