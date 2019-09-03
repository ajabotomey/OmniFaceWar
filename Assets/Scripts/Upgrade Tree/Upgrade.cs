using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public int upgradeID;
    public string upgradeName;
    public int[] upgradeDependencies;
    public bool unlocked;
    public int cost;
    public string upgradeDescription;
}
