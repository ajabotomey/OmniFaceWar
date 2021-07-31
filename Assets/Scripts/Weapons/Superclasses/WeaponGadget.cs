using UnityEngine;

public class WeaponGadget : ScriptableObject {
    [Header("Base Attributes")]
    [SerializeField] private float duration;
    [SerializeField] private float fireRate;
    [SerializeField] private float rechargeRate;
    [FMODUnity.EventRef] [SerializeField] private string weaponFireSound;

    public float Duration { get {return duration;} set {duration = value;} }
    public float FireRate { get {return fireRate;} set {fireRate = value;} }
    public float RechargeRate { get {return rechargeRate;} set {rechargeRate = value;} }
    public string WeaponFireSound { get {return weaponFireSound;} set {weaponFireSound = value;} }
}