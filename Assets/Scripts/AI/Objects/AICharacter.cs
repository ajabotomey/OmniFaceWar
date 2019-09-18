using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIType { Medic, Security, Henchman, Hunbot, Scientist, Neutral }

public abstract class AICharacter : MonoBehaviour
{
    public abstract AIType Type { get; }

    public AIContext Context { get; private set; }

    public void Init(AIContext context)
    {
        Context = context;
    }

    public void TakeDamage(int damage)
    {
        Context.TakeDamage(damage);
    }
}
