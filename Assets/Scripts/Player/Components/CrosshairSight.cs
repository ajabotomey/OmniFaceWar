﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CrosshairSight : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    private int maxDistance = 5000; // TODO: Figure out a way to confirm the sprite to the screen

    [Inject] private IInputController input;

    // Update is called once per frame
    void Update()
    {
        Vector2 aim = input.GetAim();
        //var direction = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;

        if (aim.magnitude > 0) {
            RaycastHit2D hit = Physics2D.Raycast(parent.position, aim, maxDistance, targetMask | obstacleMask);

            if (hit) {
                gameObject.SetActive(true);
                if (hit.collider) {
                    transform.position = hit.point;
                }
            } else {
                gameObject.SetActive(false);
            }
        }
    }
}