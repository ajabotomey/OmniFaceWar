﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.Analytics;

public class InitialLoad : MonoBehaviour
{
    [Header("Notifications")]
    [SerializeField] private Notification firstNotification;
    [SerializeField] private Notification movement;
    [SerializeField] private Notification rotation;
    [SerializeField] private Notification shooting;
    [SerializeField] private Notification window;
    [SerializeField] private Notification shootDoors;

    [Header("Subtitle Clips")]
    [SerializeField] private SubtitleClip movementClip;
    [SerializeField] private SubtitleClip rotationClip;
    [SerializeField] private SubtitleClip shootingClip;

    //[Header("Objectives")]
    //[SerializeField] private Objective objective;
    //[SerializeField] private ObjectivesPanel panel;

    // DI Objects
    [Inject] private CameraShake cameraShake;
    [Inject] private GameUIController gameUI;
     
    // Start is called before the first frame update
    void Start()
    {
        // Reset Notifications


        //// Setup Objectives
        //objectivesManager.SetCurrentObjective(objective);
        //((NavigationObjective)objective).ResetObjective();
        //panel.Initialize();

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

        //yield return new WaitForSeconds(3);
        //cameraShake.TriggerShake();
        //yield return new WaitForSeconds(3);
        gameUI.PushNotification(movement);
        //GameUIController.Instance.ShowSubtitles(movementClip);
        yield return new WaitForSeconds(3);
        gameUI.PushNotification(rotation);
        //GameUIController.Instance.ShowSubtitles(rotationClip);
        yield return new WaitForSeconds(3);
        gameUI.PushNotification(shooting);
        yield return new WaitForSeconds(3);
        gameUI.PushNotification(shootDoors);
        //GameUIController.Instance.ShowSubtitles(shootingClip);
        //yield return new WaitForSeconds(2);
        //GameUIController.Instance.HideSubtitles();

        yield return null;
    }
}