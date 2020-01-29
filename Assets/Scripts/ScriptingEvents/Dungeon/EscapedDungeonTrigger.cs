using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapedDungeonTrigger : MonoBehaviour
{
    [SerializeField] private VoidEvent navCompleted;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Logger.Debug("Should be triggering");

        if (!hasTriggered) {
            hasTriggered = true;
            navCompleted.Raise();
        }
    }
}
