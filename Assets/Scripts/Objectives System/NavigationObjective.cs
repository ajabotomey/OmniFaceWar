using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Navigation Objective", menuName = "Objectives System/New Navigation Objective")]
public class NavigationObjective : Objective
{
    public void ResetObjective()
    {
        isComplete = false;
    }

    public void CompleteObjective()
    {
        isComplete = true;
    }
}
