using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViralBomb : Gadget
{
    [SerializeField] private float minimumDamage = 1;
    [SerializeField] private float maximumDamage = 10;
    [SerializeField] private int numberOfBombs = 3;
}
