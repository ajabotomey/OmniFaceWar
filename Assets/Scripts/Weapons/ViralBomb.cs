using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Viral Bomb", menuName = "Weapons/New Viral Bomb")]
public class ViralBomb : Gadget
{
    [Header("Viral Bomb Attributes")]
    [SerializeField] private float minimumDamage = 1;
    [SerializeField] private float maximumDamage = 10;
    [SerializeField] private int numberOfBombs = 3;
}
