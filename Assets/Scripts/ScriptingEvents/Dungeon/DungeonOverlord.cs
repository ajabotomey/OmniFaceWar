using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonOverlord : MonoBehaviour
{
    [SerializeField] private EnemyDungeonNPC[] enemies;

    public void StopFiring()
    {
        foreach (var enemy in enemies) {
            enemy.StopFiring();
        }
    }

    public void StartFiring()
    {
        foreach (var enemy in enemies) {
            enemy.StartFiring();
        }
    }
}
