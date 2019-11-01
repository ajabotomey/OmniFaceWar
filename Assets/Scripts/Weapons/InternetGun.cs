using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Internet Gun", menuName = "Weapons/New Internet Gun")]
public class InternetGun : HackTypeGun
{
    [Header("Abilities")]
    [SerializeField] private bool spamAds;
    [SerializeField] private bool internetDisconnect;
    [SerializeField] private bool spamCatPictures;
    [SerializeField] private bool spamMLMPosts;
}
