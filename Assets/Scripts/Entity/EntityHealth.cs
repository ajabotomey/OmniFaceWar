using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [Header("Health Attributes")]
    [SerializeField] private int health;
    [SerializeField] private EntityHealthBar healthBar;

    [Header("Death Event")]
    [SerializeField] private UnityEvent deathEvent;

    private int maxHealth;

    void Awake()
    {
        if (healthBar) {
            healthBar.Init(health);
        }

        maxHealth = health;
    }

    public void ApplyDamage(int damage)
    {
        if (IsDead()) {
            deathEvent.Invoke();
            return;
        }

        health -= damage;

        if (healthBar) {
            healthBar.TakeDamage(damage);
        }
    }

    public void SetHealth(int healthValue, int maxHealth)
    {
        health = healthValue;
        if (healthBar)
            healthBar.Init(healthValue, maxHealth);
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public bool HasHealthBar()
    {
        return healthBar;
    }

    public void SetHealthBar(EntityHealthBar _healthBar)
    {
        healthBar = _healthBar;
    }
}
