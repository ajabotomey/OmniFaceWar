using UnityEngine;

public class EntityHealthBar : MonoBehaviour
{
    [SerializeField] private Transform foreground;
    private int health;
    private int maxHealth;
    private float maxWidth;
    private float height;

    public void Init(int health)
    {
        this.health = health;
        this.maxHealth = health;
        maxWidth = foreground.localScale.x;
        height = foreground.localScale.y;
    }

    public void Init(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        maxWidth = foreground.localScale.x;
        height = foreground.localScale.y;

        var damage = maxHealth - health;
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        float percent = (float)health / (float)maxHealth;
        var width = percent * maxWidth;
        foreground.localScale = new Vector2(width, height);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
