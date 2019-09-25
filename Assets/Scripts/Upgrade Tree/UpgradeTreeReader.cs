using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UpgradeTreeReader : MonoBehaviour
{
    // Array with all upgrades in a upgrade tree
    private Upgrade[] _upgradeTree;

    // Dictionary with all upgrades in upgrade tree
    private Dictionary<int, Upgrade> _upgrades;

    // variable for caching the currently being inspected skill
    private Upgrade _upgradeInspected;

    public int availablePoints = 100;

    private void Awake()
    {
        SetUpUpgradeTree();
    }

    private void SetUpUpgradeTree()
    {
        _upgrades = new Dictionary<int, Upgrade>();

        LoadUpgradeTree();
    }

    public void LoadUpgradeTree()
    {
        string path = "Assets/Data/upgradeTree.json";
        string dataAsJSON;
        if (File.Exists(path)) {
            // Read the json from the file into a string
            dataAsJSON = File.ReadAllText(path);

            // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
            UpgradeTree loadedData = JsonUtility.FromJson<UpgradeTree>(dataAsJSON);

            // Store the SkillTree as an array of Skill
            _upgradeTree = new Upgrade[loadedData.upgradeTree.Length];
            _upgradeTree = loadedData.upgradeTree;

            // Populate a dictionary will the skill id and the skill data itself
            for (int i = 0; i < _upgradeTree.Length; i++) {
                _upgrades.Add(_upgradeTree[i].upgradeID, _upgradeTree[i]);
            }
        } else {
            Debug.LogError("Cannot load Skill Tree");
        }
    }

    public bool IsUpgradeUnlocked(int skillID)
    {
        if (_upgrades.TryGetValue(skillID, out _upgradeInspected))
            return _upgradeInspected.unlocked;

        return false;
    }

    public bool CanSkillBeUnlocked(int skillID)
    {
        bool canUnlock = true;

        // Skill exists?
        if (_upgrades.TryGetValue(skillID, out _upgradeInspected)) {
            // Enough points available
            if (_upgradeInspected.cost == availablePoints) {
                int[] dependencies = _upgradeInspected.upgradeDependencies;
                for (int i = 0; i < dependencies.Length; i++) {
                    if (_upgrades.TryGetValue(dependencies[i], out _upgradeInspected)) {
                        if (!_upgradeInspected.unlocked) {
                            canUnlock = false;
                            break;
                        }
                    } else { // If one of the dependencies doesn't exist, the skill can't be unlocked
                        return false;
                    }
                }
            } else { // If the player doesn't have enough skill points, can't unlock the new skill
                return false;
            }
        } else { // If the skill id doesn't exist, the skill can't be unlocked
            return false;
        }

        return canUnlock;
    }

    public bool UnlockSkill(int skillID)
    {
        if (_upgrades.TryGetValue(skillID, out _upgradeInspected)) {
            if (_upgradeInspected.cost <= availablePoints) {
                availablePoints -= _upgradeInspected.cost;
                _upgradeInspected.unlocked = true;

                // We replace the entry on the dictionary with the new one
                _upgrades.Remove(skillID);
                _upgrades.Add(skillID, _upgradeInspected);

                return true;
            } else {
                return false;
            }
        }

        return false;
    }
}
