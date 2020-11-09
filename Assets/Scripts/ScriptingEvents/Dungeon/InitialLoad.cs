using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.Analytics;

public class InitialLoad : MonoBehaviour
{
    [Header("Notifications")]
    [SerializeField] private Notification socialScoreLow;
    [SerializeField] private Notification remediation;
    [SerializeField] private Notification movement;
    [SerializeField] private Notification rotation;
    [SerializeField] private Notification shooting;
    [SerializeField] private Notification window;
    [SerializeField] private Notification shootDoors;

    [Header("Subtitle Clips")]
    [SerializeField] private SubtitleClip movementClip;
    [SerializeField] private SubtitleClip rotationClip;
    [SerializeField] private SubtitleClip shootingClip;

    [Header("Objectives")]
    [SerializeField] private ObjectivesManager objectivesManager;
    [SerializeField] private Objective objective;
    [SerializeField] private ObjectivesPanel panel;

    [Header("Sound Sources")]
    [SerializeField] private AudioSource explosionSource;

    // DI Objects
    [Inject] private CameraShake cameraShake;
    [Inject] private GameUIController gameUI;
     
    // Start is called before the first frame update
    void Start()
    {
        // Reset Notifications


        //// Setup Objectives
        objectivesManager.SetCurrentObjective(objective);
        ((NavigationObjective)objective).ResetObjective();
        panel.Initialize();

        //GameUIController.Instance.PushNotification(firstNotification);
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        //AnalyticsEvent.Custom("level_start");
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level_index", 1);

        AnalyticsResult result = AnalyticsEvent.Custom("level_start", parameters);
        if (result == AnalyticsResult.Ok) {
            Logger.Debug("All is well!");
        } else {
            Logger.Error("We have a problem with the Analytics data");
        }

        // Initial moment
        gameUI.PushNotification(socialScoreLow);
        yield return new WaitForSeconds(3);
        gameUI.PushNotification(remediation);
        yield return new WaitForSeconds(15);
        cameraShake.TriggerShake();
        explosionSource.Play();
        yield return new WaitForSeconds(3);

        // Introduction
        gameUI.PushNotification(movement);
        gameUI.PushNotification(rotation);
        gameUI.PushNotification(shooting);
        gameUI.PushNotification(shootDoors);
        
        gameUI.ShowSubtitles(movementClip);
        yield return new WaitForSeconds(7);
        gameUI.ShowSubtitles(rotationClip);
        yield return new WaitForSeconds(7);
        gameUI.ShowSubtitles(shootingClip);
        //yield return new WaitForSeconds(2);

        //gameUI.ShowSubtitles(movementClip);

        yield return null;
    }
}
