using UnityEngine;

public class Gadget : WeaponGadget
{
    [Header("Gadget Attributes")]
    [SerializeField] private float radius;

    public float Radius { get { return radius; } set { radius = value;} }

    public override bool isGadget()
    {
        return true;
    }
}
