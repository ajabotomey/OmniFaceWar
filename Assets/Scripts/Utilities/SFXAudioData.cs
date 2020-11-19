using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFXAudioData
{
    [Header("Movement")]
    [FMODUnity.EventRef] [SerializeField] private string footsteps = null;

    [Header("Taken damage")]
    [FMODUnity.EventRef] [SerializeField] private string takenSlightDamage = null;
    [FMODUnity.EventRef] [SerializeField] private string takenMajorDamage = null;
}