using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField] private List<Objective> objectives;
    private Objective currentObjective;

    public void SetCurrentObjective(Objective objective)
    {
        // Check that the objective is in the list

        currentObjective = objective;
    }

    public void UpdateEliminationObjective()
    {
        if (currentObjective is EliminationObjective) {
            ((EliminationObjective)currentObjective).IncrementKillCount();
        }
    }

    public Objective GetCurrentObjective()
    {
        return currentObjective;
    }
}
