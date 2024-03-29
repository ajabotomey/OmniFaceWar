﻿using System.Collections;
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
    [SerializeField] private PlayerInput playerInput;
    
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

    //Current Control Scheme
    private string currentControlScheme;

    private Vector2 movement;
    private Vector2 rotation;
    private Vector3 aim;

    public Vector3 Aim {
        get { return aim; }
        set { aim = value; }
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

        currentControlScheme = playerInput.currentControlScheme;

        elapsedTime = 0.0f;
    }

    public void OnControlsChanged()
    {
        if (playerInput.currentControlScheme != currentControlScheme) {
            currentControlScheme = playerInput.currentControlScheme;
            Debug.Log("Player Control Scheme has changed!");

            ShowControllerButtonUI[] buttonPrompts = GameObject.FindObjectsOfType<ShowControllerButtonUI>();

            foreach (ShowControllerButtonUI buttonPrompt in buttonPrompts) {
                buttonPrompt.UpdateImage();
            }
        }
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
        //Debug.Log(transform.position.ToString());

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
        RotateCharacter();
        HandleMovement();
    }

    public string GetCurrentControlScheme()
    {
        return playerInput.currentControlScheme;
    }

    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }

    public float GetSpreadFactor()
    {
        return spreadFactor;
    }

    #region Unity Input System Callbacks

    public void ReadMovement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void ReadRotation(InputAction.CallbackContext value)
    {
        rotation = value.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext value)
    {
        if (value.started)
            CheckForNearbyNPC();
    }

    #endregion

    void RotateCharacter()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void HandleMovement()
    {
        float posX = transform.position.x + movement.x * _speed * Time.fixedDeltaTime;
        float posY = transform.position.y + movement.y * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(new Vector2(posX, posY));
    }

    void SetAim()
    {
        if (aimTarget) {
            // TODO: Might add a timer for the auto aim so it only snaps every few seconds perhaps


            var temp = aimTarget.position - transform.position;
            temp.Normalize();
            aim = temp;
        } else if (playerInput.currentControlScheme == "Gamepad") { // Fix this in case the gamepad is connected but I still want to use the mouse
            aim = rotation;
            if (aim.magnitude > 1.0f)
                aim.Normalize();
        } else {
            aim += new Vector3(rotation.x, rotation.y,  0.0f);
            if (aim.magnitude > 1.0f) {
                aim.Normalize();
            }
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

        if (aim.magnitude > 0.0f && _weaponControl.GetCurrentWeapon() != null) {
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
            //playerInput.SwitchCurrentActionMap("UI Controls");
            _gameUI.StartConversation(target.talkToNode);
        }
    }

    public void SwitchBackToPlayerControls()
    {
        playerInput.SwitchCurrentActionMap("Player Controls");
    }

}
