using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : ScriptableObject
{
    [SerializeField] protected string title;
    [SerializeField] protected string description;
    // Maybe add some experience later

    [SerializeField] protected bool isComplete;

    public string GetTitle()
    {
        return title;
    }

    public string GetDescription()
    {
        return description;
    }

    public bool IsComplete()
    {
        return isComplete;
    }
}
