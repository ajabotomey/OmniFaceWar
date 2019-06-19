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

    [Inject]
    public void Construct(IInputController inputController)
    {
        _inputController = inputController;
    }

    void FixedUpdate()
    {
        RotateCharacter();
        HandleMovement();
    }

    void RotateCharacter()
    {
        if (_inputController.IsControllerActive()) {
            var angle = _inputController.Rotation();

            if (angle == 0) {
                _rb.MoveRotation(0);
            } else {
                var angleWithOffset = angle - _offset;
                _rb.MoveRotation(angleWithOffset);
            }
        } else {
            Vector3 difference = Camera.main.ScreenToWorldPoint(_inputController.MousePosition()) - transform.position;
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            float rotationWithOffset = rotation_z + _offset;

            _rb.MoveRotation(rotationWithOffset);
        }
    }

    void HandleMovement()
    {
        float moveHorizontal = _inputController.Horizontal();
        float moveVertical = _inputController.Vertical();

        float posX = transform.position.x + moveHorizontal * _speed * Time.fixedDeltaTime;
        float posY = transform.position.y + moveVertical * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(new Vector2(posX, posY));
    }

    public void RotateTowardsTarget(Transform target)
    {
        var dir = target.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var angleWithOffset = angle - _offset;
        _rb.MoveRotation(angleWithOffset);
    }
}
