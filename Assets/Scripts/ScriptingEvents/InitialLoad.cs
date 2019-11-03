using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLoad : MonoBehaviour
{
    [Header("Notifications")]
    [SerializeField] private Notification firstNotification;
    [SerializeField] private Notification movement;
    [SerializeField] private Notification rotation;
    [SerializeField] private Notification shooting;
    [SerializeField] private Notification window;

    [Header("Subtitle Clips")]
    [SerializeField] private SubtitleClip movementClip;
    [SerializeField] private SubtitleClip rotationClip;
    [SerializeField] private SubtitleClip shootingClip;

    // Start is called before the first frame update
    void Start()
    {
        // Reset tutorial notifications

        //GameUIController.Instance.PushNotification(firstNotification);
        StartCoroutine(PushNotifications());
    }

    IEnumerator PushNotifications()
    {
        GameUIController.Instance.PushNotification(movement);
        GameUIController.Instance.ShowSubtitles(movementClip);
        yield return new WaitForSeconds(3);
        GameUIController.Instance.PushNotification(rotation);
        GameUIController.Instance.ShowSubtitles(rotationClip);
        yield return new WaitForSeconds(3);
        GameUIController.Instance.PushNotification(shooting);
        GameUIController.Instance.ShowSubtitles(shootingClip);
        yield return new WaitForSeconds(2);
        GameUIController.Instance.HideSubtitles();

        yield return null;
    }
}
