using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class GunComponent : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private GameObject bullet;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask targetMask;

    private float elapsedTime;

    private IInputController _inputController;
    private Bullet.Factory _bulletFactory;
    private PlayerControl _player;

    [Inject]
    public void Construct(IInputController inputController, Bullet.Factory bulletFactory, PlayerControl playerControl)
    {
        _inputController = inputController;
        _bulletFactory = bulletFactory;
        _player = playerControl;
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        bool fireBullet = _inputController.FireWeapon();

        if (fireBullet && elapsedTime >= fireRate) {
            Fire();
        }

        elapsedTime += Time.deltaTime;
    }

    void Fire()
    {
        //GameObject firedBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet firedBullet = _bulletFactory.Create();
        firedBullet.transform.position = bulletSpawnPoint.position;
        firedBullet.transform.rotation = bulletSpawnPoint.rotation;
        firedBullet.GetComponent<Rigidbody2D>().AddForce(transform.up * 500.0f);

        elapsedTime = 0.0f;
    }
}
