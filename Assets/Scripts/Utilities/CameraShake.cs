using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeAmplitude = 1.2f;
    [SerializeField] private float shakeFrequency = 2.0f;

    private float shakeElapsedTime = 0.0f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    // Start is called before the first frame update
    void Start()
    {
        if (Equals(virtualCamera, null)) {
            Logger.Error("The Virtual Camera has not been connected to the Camera Shake object!");
            Application.Quit();
        }

        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Equals(virtualCamera, null) && !Equals(virtualCameraNoise, null)) {
            // If Camera shake is still playing
            if (shakeElapsedTime > 0) {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;

                // Update Shake timer
                shakeElapsedTime -= Time.deltaTime;
            } else {
                // If camera shake is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
        }
    }

    public void TriggerShake()
    {
        shakeElapsedTime = shakeDuration;
    }
}
