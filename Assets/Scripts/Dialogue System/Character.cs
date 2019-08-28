using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue System/New Character")]
public class Character : ScriptableObject
{
    [SerializeField] private string fullName;

    public string GetFullName()
    {
        return fullName;
    }
}
