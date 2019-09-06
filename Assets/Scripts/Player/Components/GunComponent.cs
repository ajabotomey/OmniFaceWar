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

    [SerializeField] private Rigidbody2D rb;

    private float elapsedTime;

    private IInputController _inputController;
    private Bullet.Factory _bulletFactory;
    private GameUIController _gameUI;
    private WeaponController _weaponControl;

    private float _offset = -90.0f;

    [Inject]
    public void Construct(IInputController inputController, Bullet.Factory bulletFactory, GameUIController gameUI, WeaponController weaponControl)
    {
        _inputController = inputController;
        _bulletFactory = bulletFactory;
        _gameUI = gameUI;
        _weaponControl = weaponControl;
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        _weaponControl.SelectPistol();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateComponent();

        bool fireBullet = _inputController.FireWeapon();

        if (!_gameUI.IsConversationActive()) {
            if (fireBullet) {
                Fire();
            }
        }

        elapsedTime += Time.deltaTime;
    }

    void Fire()
    {
        // Retrieve current weapon
        Weapon currentWeapon = _weaponControl.GetCurrentWeapon();
        if (currentWeapon == null)
            return;

        // Check fire rate
        if (elapsedTime >= currentWeapon.GetFireRate()) {
            Bullet firedBullet = _bulletFactory.Create();
            firedBullet.transform.position = bulletSpawnPoint.position;
            firedBullet.transform.rotation = bulletSpawnPoint.rotation;
            firedBullet.GetComponent<Rigidbody2D>().AddForce(rb.transform.up * 500f);

            elapsedTime = 0.0f;
        }
    }

    void RotateComponent()
    {
        if (_inputController.IsControllerActive()) {
            var angle = _inputController.RotationAtan();

            if (angle == 0) {
                rb.MoveRotation(0);
            } else {
                var angleWithOffset = angle - _offset;
                rb.MoveRotation(angleWithOffset);
            }
        } else {
            Vector3 difference = Camera.main.ScreenToWorldPoint(_inputController.MousePosition()) - transform.position;
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            float rotationWithOffset = rotation_z + _offset;

            //gunObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationWithOffset));
            rb.MoveRotation(rotationWithOffset);
        }
    }
}
