using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLoad : MonoBehaviour
{
    [SerializeField] private Notification firstNotification;

    // Start is called before the first frame update
    void Start()
    {
        GameUIController.Instance.PushNotification(firstNotification);
    }
}
