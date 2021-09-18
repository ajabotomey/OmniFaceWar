using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyDungeonNPC : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [Tooltip("X is horizontal, Y is Vertical. - is left/down, + is right/up")][SerializeField] private Vector2 direction;

    private float elapsedTime;

    private Bullet.Factory _factory;

    private bool isAllowToFire = true;

    [Inject]
    public void Construct(Bullet.Factory bulletFactory) {
        _factory = bulletFactory;
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 3.0f;
    }

    public void StopFiring()
    {
        isAllowToFire = false;
    }

    public void StartFiring()
    {
        isAllowToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAllowToFire) {
            if (elapsedTime > fireRate) {
                Bullet bullet = _factory.Create();
                bullet.transform.position = transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = direction * 50.0f;

                elapsedTime = 0.0f;
            }

            elapsedTime += Time.deltaTime;
        }
    }
}
