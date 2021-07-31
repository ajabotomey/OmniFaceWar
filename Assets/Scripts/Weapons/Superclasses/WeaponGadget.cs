using UnityEngine;

public class WeaponGadget : ScriptableObject {
    [Header("Base Attributes")]
    [SerializeField] private float duration;
    [SerializeField] private float fireRate;
    [SerializeField] private float rechargeRate;
    [SerializeField] private bool isCurrentlySelected;
    [FMODUnity.EventRef] [SerializeField] private string weaponFireSound;

    public float Duration { get {return duration;} set {duration = value;} }
    public float FireRate { get {return fireRate;} set {fireRate = value;} }
    public float RechargeRate { get {return rechargeRate;} set {rechargeRate = value;} }
    public string WeaponFireSound { get {return weaponFireSound;} set {weaponFireSound = value;} }
    public bool IsCurrentlySelected { get {return isCurrentlySelected;} set {isCurrentlySelected = value;} }

    public virtual bool isWeapon() { return false; }
    public virtual bool isGadget() { return false;}

    public void Select()
    {
        isCurrentlySelected = true;
    }

    public void Deselect()
    {
        isCurrentlySelected = false;
    }
}