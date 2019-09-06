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

    private bool hasGun = false;

    //[SerializeField] private SpriteController spriteController;
    [SerializeField] private Animator animator;

    private bool isInConversation;

    [SerializeField] private GameObject crosshair;

    Vector3 aim;

    [Inject]
    public void Construct(IInputController inputController)
    {
        _inputController = inputController;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoveCrosshair();
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
        Logger.Debug("Rotating Character");
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Vector2 movement = new Vector2(horizontal, vertical);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void HandleMovement(float horizontal, float vertical)
    {
        Logger.Debug("Moving Character");
        float posX = transform.position.x + horizontal * _speed * Time.fixedDeltaTime;
        float posY = transform.position.y + vertical * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(new Vector2(posX, posY));
    }

    // Mouse only for the moment
    void MoveCrosshair()
    {
        Vector3 mouseMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0f);
        aim += mouseMovement;

        if (aim.magnitude > 1.0f) {
            aim.Normalize();
        }

        if (aim.magnitude > 0.0f) {
            crosshair.transform.localPosition = aim * 2;
            crosshair.SetActive(true);
        } else {
            crosshair.SetActive(false);
        }

        _inputController.SetAim(aim);
    }
}
