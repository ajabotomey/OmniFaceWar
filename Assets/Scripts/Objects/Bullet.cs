using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private int damage;

    private IInputController _inputController;
    private SettingsManager _settingsManager;

    [Inject]
    public void Construct(IInputController inputController, SettingsManager settings)
    {
        _inputController = inputController;
        _settingsManager = settings;
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

            if (_settingsManager.IsRumbleEnabled())
                _inputController.SetRumble(1.0f);
        }

        if (collision.gameObject.tag == "Destructible Object") {
            collision.gameObject.GetComponent<DestructibleObject>().TakeDamage(damage);

            if (_settingsManager.IsRumbleEnabled())
                _inputController.SetRumble(0.2f);
        }

        Destroy(gameObject);
    }

    public class Factory : PlaceholderFactory<Bullet>{ }
}
