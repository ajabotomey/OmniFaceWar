﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NoisemakerComponent : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float fireRate = 10.0f;
    [SerializeField] private GameObject noisemaker;
    [SerializeField] private float zSpeed = 0;
    [SerializeField] private LayerMask obstacleMask;

    private float elapsedTime;
    private float maxDistance = 5000;

    private IInputController _inputController;
    private NoisemakerObject.Factory _noisemakerFactory;
    private WeaponController _weaponControl;

    [Inject]
    public void Construct(IInputController inputController, NoisemakerObject.Factory noisemakerFactory, WeaponController weaponControl)
    {
        _inputController = inputController;
        _noisemakerFactory = noisemakerFactory;
        _weaponControl = weaponControl;
    }

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = fireRate;
    }


    // Update is called once per frame
    void Update()
    {
        // Check if the noisemaker is selected first
        if (_weaponControl.GetCurrentGadget() == null || _weaponControl.GetCurrentGadget().GetType() != typeof(Noisemaker))
            return;

        // TODO: Fix once Weapon Control is available
        //bool launchNoise = _inputController.FireWeapon();
        //bool launchNoise = Input.GetKeyDown(KeyCode.E);
        bool launchNoise = false;

        // Check that we aren't throwing it onto an object. Also make sure we aren't throwing over obstacles as well
        var mousePos = Camera.main.ScreenToWorldPoint(_inputController.MousePosition());
        var distance = Vector2.Distance(transform.position, mousePos);
        RaycastHit2D initialHit = Physics2D.Raycast(mousePos, Vector2.zero, maxDistance, obstacleMask);
        RaycastHit2D obstructionHit = Physics2D.Raycast(transform.position, transform.up, distance, obstacleMask);

        if (!initialHit && !obstructionHit) {
            if (launchNoise) {
                Fire(distance, mousePos);
            }
        } else {
            // Change the cursor
        }
    }

    void Fire(float distance, Vector3 mousePos)
    {
        // TODO: If it is a small distance, we might as well drop it at the position as opposed to throwing it
        //if (distance <= 3.0f) {
        //    Noisemaker noise = Instantiate(noisemaker, mousePos, Quaternion.identity).GetComponent<Noisemaker>();
        //    noise.Landed();
        //} else {
        //Noisemaker noise = Instantiate(noisemaker, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Noisemaker>();
        NoisemakerObject noise = _noisemakerFactory.Create();
        noise.transform.position = bulletSpawnPoint.position;
        noise.transform.rotation = Quaternion.identity;
        noise.Init(distance, mousePos);
        //}
    }
}
