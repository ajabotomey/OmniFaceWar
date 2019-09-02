using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public SafeInt upgradeID;
    public string upgradeName;
    public SafeInt[] upgradeDependencies;
    public bool unlocked;
    public SafeInt cost;
    public string upgradeDescription;
}
