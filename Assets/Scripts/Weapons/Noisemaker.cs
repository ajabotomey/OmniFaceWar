using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Noisemaker", menuName = "Weapons/New Noisemaker")]
public class Noisemaker : Gadget
{
    [Header("Noisemaker Attributes")]
    [SerializeField] private LayerMask obstacleMask;
}
