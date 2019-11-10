using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;

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

    [Header("Objectives")]
    [SerializeField] private Objective objective;
    [SerializeField] private ObjectivesPanel panel;

    // DI Objects
    [Inject] private CameraShake cameraShake;
    [Inject] private ObjectivesManager objectivesManager;
    [Inject] private GameUIController gameUI;
     
    // Start is called before the first frame update
    void Start()
    {
        // Reset Notifications


        // Setup Objectives
        objectivesManager.SetCurrentObjective(objective);
        ((EliminationObjective)objective).ResetObjective();
        panel.Initialize();

        // Reset tutorial notifications

        //GameUIController.Instance.PushNotification(firstNotification);
        StartCoroutine(PushNotifications());
    }

    IEnumerator PushNotifications()
    {
        //yield return new WaitForSeconds(3);
        //cameraShake.TriggerShake();
        //yield return new WaitForSeconds(3);
        gameUI.PushNotification(movement);
        //GameUIController.Instance.ShowSubtitles(movementClip);
        //yield return new WaitForSeconds(3);
        //GameUIController.Instance.PushNotification(rotation);
        //GameUIController.Instance.ShowSubtitles(rotationClip);
        //yield return new WaitForSeconds(3);
        //GameUIController.Instance.PushNotification(shooting);
        //GameUIController.Instance.ShowSubtitles(shootingClip);
        //yield return new WaitForSeconds(2);
        //GameUIController.Instance.HideSubtitles();

        yield return null;
    }
}
