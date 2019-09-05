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

    [Inject]
    public void Construct(IInputController inputController)
    {
        _inputController = inputController;
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
        //// Original rotation
        //if (_inputController.IsControllerActive()) {
        //    var angle = _inputController.Rotation();

        //    if (angle == 0) {
        //        _rb.MoveRotation(0);
        //    } else {
        //        var angleWithOffset = angle - _offset;
        //        _rb.MoveRotation(angleWithOffset);
        //    }
        //} else {
        //    Vector3 difference = Camera.main.ScreenToWorldPoint(_inputController.MousePosition()) - transform.position;
        //    difference.Normalize();
        //    float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        //    float rotationWithOffset = rotation_z + _offset;

        //    _rb.MoveRotation(rotationWithOffset);
        //}

        //spriteController.RotateSprite(horizontal, vertical);

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
}
