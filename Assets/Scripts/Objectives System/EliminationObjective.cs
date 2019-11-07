using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Elimination Objective", menuName="Objectives System/New Elimination Objective")]
public class EliminationObjective : Objective
{
    [SerializeField] private int killCountRequired = 0;
    [SerializeField] private int currentKillCount = 0;

    public void ResetObjective()
    {
        currentKillCount = 0;
        isComplete = false;
    }

    public void IncrementKillCount()
    {
        if (isComplete)
            return;

        currentKillCount++;

        if (currentKillCount == killCountRequired)
            isComplete = true;
    }

    public int GetKillCount()
    {
        return currentKillCount;
    }

    public int GetKillsRequired()
    {
        return killCountRequired;
    }
}
