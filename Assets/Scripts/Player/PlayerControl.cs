using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 1.0f;

    private float _offset = -90.0f;

    private IInputController _inputController;
    private GameUIController _gameUI;
    private HeatmapUploadController _heatmap;

    private bool hasGun = false;

    //[SerializeField] private SpriteController spriteController;
    [SerializeField] private Animator animator;

    private bool isInConversation;

    [SerializeField] private GameObject crosshair;

    private float refreshRate = 10.0f;
    private float elapsedTime;

    Vector3 aim;

    private Transform aimTarget = null;

    [Inject]
    public void Construct(IInputController inputController, GameUIController gameUI, HeatmapUploadController heatmap)
    {
        _inputController = inputController;
        _gameUI = gameUI;
        _heatmap = heatmap;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        elapsedTime = 0.0f;
    }

    void Update()
    {
        if (_gameUI.IsInteractingWithUI()) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            crosshair.SetActive(false);
        } else {
            SetAim();
            MoveCrosshair();
        }
        
        // Update heatmap position
        if (elapsedTime >= refreshRate) {
            _heatmap.AddPosition(transform.position);
            elapsedTime = 0.0f;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = _inputController.Horizontal();
        float moveVertical = _inputController.Vertical();

        RotateCharacter(moveHorizontal, moveVertical);
        HandleMovement(moveHorizontal, moveVertical);      
    }

    void RotateCharacter(float horizontal, float vertical)
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Vector2 movement = new Vector2(horizontal, vertical);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void HandleMovement(float horizontal, float vertical)
    {
        float posX = transform.position.x + horizontal * _speed * Time.fixedDeltaTime;
        float posY = transform.position.y + vertical * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(new Vector2(posX, posY));
    }

    void SetAim()
    {
        if (aimTarget) {
            // TODO: Might add a timer for the auto aim so it only snaps every few seconds perhaps

            var temp = aimTarget.position - transform.position;
            temp.Normalize();
            aim = temp;
        } else if (_inputController.IsControllerActive()) {
            aim = _inputController.Rotation3Raw();
            aim.Normalize();
            aim = -aim;
        } else {
            Vector3 mouseMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0f);
            aim += mouseMovement;

            if (aim.magnitude > 1.0f) {
                aim.Normalize();
            }
        }

        _inputController.SetAim(aim);
        aimTarget = null;
    }

    void MoveCrosshair()
    {
        if (Cursor.visible) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (aim.magnitude > 0.0f) {
            crosshair.transform.localPosition = aim * 2;
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
}
