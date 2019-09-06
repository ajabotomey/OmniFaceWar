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

    [SerializeField] private GameObject gunObj;
    [SerializeField] private GameObject crosshair;

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
        bool fireBullet = _inputController.FireWeapon();

        RotateComponent();

        if (!_gameUI.IsConversationActive()) {
            if (fireBullet) {
                Fire();
            }
        }

        elapsedTime += Time.deltaTime;
    }

    void Fire()
    {
        //Get Direction
        Vector2 aim = _inputController.GetAim();

        // Retrieve current weapon
        Weapon currentWeapon = _weaponControl.GetCurrentWeapon();
        if (currentWeapon == null)
            return;

        // Make sure we are actually aiming first
        if (aim.magnitude > 0.0f) {
            // Check fire rate
            if (elapsedTime >= currentWeapon.GetFireRate()) {
                Bullet firedBullet = _bulletFactory.Create();
                firedBullet.transform.position = bulletSpawnPoint.position;
                firedBullet.transform.rotation = Quaternion.identity;
                firedBullet.GetComponent<Rigidbody2D>().velocity = aim * 50.0f;
                firedBullet.transform.Rotate(0, 0, Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg);

                elapsedTime = 0.0f;
            }
        }
    }

    // Mouse only for the moment
    void RotateComponent()
    {
        Vector2 target = crosshair.transform.position;
        Vector2 gunPos = gunObj.transform.position;
        target.x = target.x - gunPos.x;
        target.y = target.y - gunPos.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        angle -= 90;
        gunObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
