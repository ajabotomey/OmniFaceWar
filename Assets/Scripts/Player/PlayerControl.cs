using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float interactionRadius = 2.0f;
    [SerializeField] private float spreadFactor = 0.1f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private EntityHealth health;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Animator animator;
    
    // Injected objects
    private IInputController _inputController;
    private GameUIController _gameUI;
    private HeatmapUploadController _heatmap;
    private WeaponController _weaponControl;

    private float _offset = -90.0f;
    private bool hasGun = false;
    private bool isInConversation;
    private float refreshRate = 10.0f;
    private float elapsedTime;

    private Vector2 movement;
    private Vector2 rotation;

    public Vector3 Aim {
        get; set;
    }

    private Transform aimTarget = null;

    [Inject]
    public void Construct(IInputController inputController, GameUIController gameUI, HeatmapUploadController heatmap, WeaponController weaponControl)
    {
        _inputController = inputController;
        _gameUI = gameUI;
        _heatmap = heatmap;
        _weaponControl = weaponControl;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //healthController = new EntityHealth(health);

        elapsedTime = 0.0f;
    }

    //[Inject]
    //public void Construct(IInputController inputController)
    //{
    //    _inputController = inputController;

    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;

    //    elapsedTime = 0.0f;
    //}

    void Update()
    {
        if (!health.HasHealthBar())
            health.SetHealthBar(_gameUI.GetHealthBar());

        if (_gameUI.IsInteractingWithUI()) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            crosshair.SetActive(false);
        } else {
            SetAim();
            MoveCrosshair();
        }

        // Update heatmap position
        _heatmap.AddPosition(transform.position);
    }

    void FixedUpdate()
    {
        //float moveHorizontal = _inputController.Horizontal();
        //float moveVertical = _inputController.Vertical();

        RotateCharacter();
        HandleMovement();
        //HandleMovement(moveHorizontal, moveVertical);
        //HandleMovement(_inputController.Movement().x, _inputController.Movement().y);

        if (_inputController.TalkToNPC()) {
            CheckForNearbyNPC();
        }
    }

    public float GetSpreadFactor()
    {
        return spreadFactor;
    }

    void RotateCharacter(float horizontal, float vertical)
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Vector2 movement = new Vector2(horizontal, vertical);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void RotateCharacter()
    {
        animator.SetFloat("Horizontal", rotation.x);
        animator.SetFloat("Vertical", rotation.y);

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void HandleMovement(float horizontal, float vertical)
    {
        float posX = transform.position.x + horizontal * _speed * Time.fixedDeltaTime;
        float posY = transform.position.y + vertical * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(new Vector2(posX, posY));
    }

    public void ReadMovement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void ReadRotation(InputAction.CallbackContext value)
    {
        rotation = value.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        float posX = transform.position.x + movement.x * _speed * Time.fixedDeltaTime;
        float posY = transform.position.y + movement.y * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(new Vector2(posX, posY));
    }

    void SetAim()
    {
        //if (aimTarget) {
        //    // TODO: Might add a timer for the auto aim so it only snaps every few seconds perhaps

        //    var temp = aimTarget.position - transform.position;
        //    temp.Normalize();
        //    aim = temp;
        //} else if (_inputController.IsControllerActive()) {
        //    aim = _inputController.Rotation3Raw();
        //    aim.Normalize();
        //    aim = -aim;
        //} else {
        //    //Vector3 mouseMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0f);
        //    Vector3 mouseMovement = new Vector3(_inputController.MousePosition().x, _inputController.MousePosition().y, 0.0f);
        //    aim += mouseMovement;

        //    if (aim.magnitude > 1.0f) {
        //        aim.Normalize();
        //    }
        //}

        if (aimTarget) {
            // TODO: Might add a timer for the auto aim so it only snaps every few seconds perhaps


            var temp = aimTarget.position - transform.position;
            temp.Normalize();
            Aim = temp;
        } else if (Gamepad.current != null) {
            Aim = rotation;
            if (Aim.magnitude > 1.0f)
                Aim.Normalize();
        } else {
            var mousePos = Mouse.current.position.ReadValue();
            Aim += new Vector3(mousePos.x, mousePos.y, 0.0f);
            if (Aim.magnitude > 1.0f)
                Aim.Normalize();
        }

        //_inputController.SetAim(Aim);
        aimTarget = null;
    }

    void MoveCrosshair()
    {
        if (Cursor.visible) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Aim.magnitude > 0.0f && _weaponControl.GetCurrentWeapon() != null) {
            crosshair.transform.localPosition = Aim * 2;
            crosshair.SetActive(true);
        } else {
            crosshair.SetActive(false);
        }
    }

    public void SetAimTarget(Transform target = null)
    {
        if (target == null) {
            aimTarget = null;
        } else {
            aimTarget = target;
        }
    }

    public void InitiateCombat()
    {
        _gameUI.FadeOutElementsForCombat();
    }

    public void CombatEnded()
    {
        _gameUI.FadeInElementsAfterCombat();
    }

    public void CheckForNearbyNPC()
    {
        var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
        var target = allParticipants.Find(delegate (NPC p) {
            return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
            (p.transform.position - this.transform.position)// is in range?
            .magnitude <= interactionRadius;
        });
        if (target != null) {
            // Kick off the dialogue at this node.
            _gameUI.StartConversation(target.talkToNode);
        }
    }
}
