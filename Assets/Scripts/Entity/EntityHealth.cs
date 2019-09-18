using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth
{
    private int health;
    private int maxHealth;

    private void Initialize(int _health)
    {
        health = _health;
        maxHealth = _health;
    }

    public void ApplyDamage(int damage)
    {
        if (IsDead())
            return;

        health -= damage;
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
