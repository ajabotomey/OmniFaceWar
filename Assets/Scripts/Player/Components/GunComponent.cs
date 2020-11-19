using UnityEngine;
using Zenject;

public class GunComponent : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private GameObject bullet;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask targetMask;

    [SerializeField] private GameObject gunObj;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private AudioSource audioSource;

    private float fireElapsedTime;
    private float autoAimElapsedTime;

    private IInputController _inputController;
    private Bullet.Factory _bulletFactory;
    private GameUIController _gameUI;
    private WeaponController _weaponControl;
    private SettingsManager _settings;
    private PlayerControl _player;

    private float _offset = -90.0f;

    private float autoAimDelay = 2.0f;

    // Real one
    [Inject]
    public void Construct(IInputController inputController, Bullet.Factory bulletFactory, GameUIController gameUI, WeaponController weaponControl, SettingsManager settings, PlayerControl player)
    {
        _inputController = inputController;
        _bulletFactory = bulletFactory;
        _gameUI = gameUI;
        _weaponControl = weaponControl;
        _settings = settings;
        _player = player;
    }

    //[Inject]
    //public void Construct(IInputController inputController, Bullet.Factory bulletFactory, WeaponController weaponControl, SettingsManager settings, TestPlayerControl player)
    //{
    //    _inputController = inputController;
    //    _bulletFactory = bulletFactory;
    //    _weaponControl = weaponControl;
    //    _settings = settings;
    //    _player = player;
    //}

    // Start is called before the first frame update
    void Start()
    {
        fireElapsedTime = 0.0f;
        //_weaponControl.SelectPistol();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool fireBullet = _inputController.FireWeapon();

        RotateComponent();

        if (_settings.IsAutoAimEnabled()) {
            var autoAimStrength = _settings.GetAutoAimStrength();
            var direction = gunObj.transform.rotation * Vector2.up;
            var target = LookForEnemyWithThickRaycast(bulletSpawnPoint.position, direction, autoAimStrength);

            // Snap crosshair to target
            if (target && autoAimElapsedTime > autoAimDelay) {
                _player.SetAimTarget(target);
                Logger.Debug("Aimbot targetting enemy");
                autoAimElapsedTime = 0.0f;
            }
        }

        if (!_gameUI.IsInteractingWithUI()) {
            if (fireBullet) {
                Fire();
            }
        }

        _weaponControl.RechargeGuns();

        fireElapsedTime += Time.fixedDeltaTime;
        autoAimElapsedTime += Time.fixedDeltaTime;
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
            if (fireElapsedTime >= currentWeapon.GetFireRate()) {
                // Check if gun has enough energy, make sure it is also a Bullet Type Gun as well
                if (currentWeapon.CanWeaponFire() && currentWeapon is BulletTypeGun) {
                    Bullet firedBullet = _bulletFactory.Create();

                    int damage = ((BulletTypeGun)currentWeapon).Damage;
                    firedBullet.SetDamage(damage);

                    // Setup audio clip
                    //audioSource.clip = currentWeapon.GetWeaponFireSound();

                    // Incorporate spread into the aim
                    float spreadFactor = _player.GetSpreadFactor();
                    aim.x += Random.Range(-spreadFactor, spreadFactor);

                    firedBullet.transform.position = bulletSpawnPoint.position;
                    firedBullet.transform.rotation = Quaternion.identity;
                    firedBullet.GetComponent<Rigidbody2D>().velocity = aim * 50.0f;
                    firedBullet.transform.Rotate(0, 0, Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg);

                    currentWeapon.Fire();
                    //audioSource.Play();

                    if (_settings.IsRumbleEnabled())
                        _inputController.SetRumble(0.1f);

                    fireElapsedTime = 0.0f;
                } else {
                    // Energy Bar outline should flash red briefly
                }
            }
        }
    }

    void RotateComponent()
    {
        RotateTowardsTarget(crosshair.transform);
    }

    public void RotateTowardsTarget(Transform target)
    {
        Vector2 targetPos = target.position;
        Vector2 gunPos = gunObj.transform.position;
        targetPos.x = targetPos.x - gunPos.x;
        targetPos.y = targetPos.y - gunPos.y;

        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        angle -= 90;
        gunObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public Transform LookForEnemyWithThickRaycast(Vector2 startWorldPos, Vector2 direction, float visibilityThickness)
    {
        if (visibilityThickness == 0) return null; //aim assist disabled

        int[] castOrder = { 2, 1, 3, 0, 4 };
        //int[] castOrder = { 2, 1, 0 };
        int numberOfRays = castOrder.Length;

        // TODO: Add these values to Settings so players can customise
        const float minDistanceAway = 2.5f; //don't find results closer than this
        const float castDistance = 10f;
        const float flareOutAngle = 15f;

        Transform target = null;
        foreach (int i in castOrder) {
            Vector2 perpDirection = Vector2.Perpendicular(direction);
            float perpDistance = (-visibilityThickness * 0.5f + i * visibilityThickness / (numberOfRays - 1)) / 64;
            Vector2 startPos = perpDirection * perpDistance + startWorldPos;

            float angleOffset = -flareOutAngle * 0.5f + i * flareOutAngle / (numberOfRays - 1);
            Vector2 flaredDirection = direction.Rotate(angleOffset);

            RaycastHit2D hit = Physics2D.Raycast(startPos, flaredDirection, castDistance, targetMask | obstacleMask);
            Debug.DrawRay(startPos, flaredDirection * castDistance, Color.yellow, Time.deltaTime);
            if (hit) {
                if (IsInTargetLayer(hit.collider.gameObject.layer)) {
                    //make sure it's in range
                    float distanceAwaySqr = (hit.transform.position.toVector2() - startWorldPos).sqrMagnitude;
                    if (distanceAwaySqr > minDistanceAway * minDistanceAway) {
                        Debug.DrawRay(startPos, direction * castDistance, Color.red, Time.deltaTime);
                        target = hit.transform;
                        return target;
                    }
                }
            }
        }

        return target;
    }

    private bool IsInTargetLayer(int layer)
    {
        var targetLayer = (int)Mathf.Log(targetMask.value, 2);

        return layer == targetLayer;
    }
}
