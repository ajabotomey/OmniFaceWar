using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityHealth))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TargetDummy : MonoBehaviour
{
    [SerializeField] private EntityHealth _health;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void TakeDamage(int damage)
    {
        _health.ApplyDamage(damage);
    }

    public void OnDead()
    {
        _spriteRenderer.color = Color.red;
    }
}
