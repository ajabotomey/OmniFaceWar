using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CrosshairSight : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private SpriteRenderer renderer;

    private int maxDistance = 5000; // TODO: Figure out a way to confirm the sprite to the screen

    //[Inject] private IInputController input;
    [Inject] private PlayerControl player;
    [Inject] private WeaponController weaponControl;

    // Update is called once per frame
    void Update()
    {
        if (weaponControl.GetCurrentWeapon() == null)
        {
            renderer.enabled = false;
            return;
        }

        Vector2 aim = player.Aim;
        //var direction = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;

        if (aim.magnitude > 0) {
            RaycastHit2D hit = Physics2D.Raycast(parent.position, aim, maxDistance, targetMask | obstacleMask);

            if (hit) {
                renderer.enabled = true;
                if (hit.collider) {
                    transform.position = hit.point;
                }
            } else {
                renderer.enabled = false;
            }
        }
    }
}
