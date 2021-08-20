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

        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<AICharacter>().TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Destructible Object") {

            DestructibleTileMap damageable = collision.gameObject.GetComponent<DestructibleTileMap>();
            if (!Equals(damageable, null)) {
                // Retrieve array of contacts first
                ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
                collision.GetContacts(contacts);

                Vector3 hitPosition = Vector3.zero;
                // For each contact in collision
                foreach (var contact in contacts) {
                    //damageable.Damage(contact.point, damage); // Original Line

                    hitPosition.x = contact.point.x - 0.01f * contact.normal.x;
                    hitPosition.y = contact.point.y - 0.01f * contact.normal.y;

                    damageable.Damage(hitPosition, damage);
                }
            }

            if (_settingsManager.IsRumbleEnabled())
                _inputController.SetRumble(0.2f);
        }

        Destroy(gameObject);
    }

    public class Factory : PlaceholderFactory<Bullet>{ }
}
