using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private int damage;

    private IInputController _inputController;

    [Inject]
    public void Construct(IInputController inputController)
    {
        _inputController = inputController;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;

        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player") {
            //collision.gameObject.GetComponent<EntityHealth>().ApplyDamage(damage);
            _inputController.SetRumble(1.0f);
        }

        if (collision.gameObject.tag == "Destructible Object") {
            collision.gameObject.GetComponent<DestructibleObject>().TakeDamage(damage);
            _inputController.SetRumble(0.2f);
        }

        Destroy(gameObject);
    }

    public class Factory : PlaceholderFactory<Bullet>{ }
}
