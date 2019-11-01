using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLoad : MonoBehaviour
{
    [SerializeField] private Notification firstNotification;
    [SerializeField] private Notification movement;
    [SerializeField] private Notification rotation;
    [SerializeField] private Notification shooting;
    [SerializeField] private Notification window;

    // Start is called before the first frame update
    void Start()
    {
        //GameUIController.Instance.PushNotification(firstNotification);
        StartCoroutine(PushNotifications());
    }

    IEnumerator PushNotifications()
    {
        GameUIController.Instance.PushNotification(movement);
        yield return new WaitForSeconds(3);
        GameUIController.Instance.PushNotification(rotation);
        yield return new WaitForSeconds(3);
        GameUIController.Instance.PushNotification(shooting);

        yield return null;
    }
}
