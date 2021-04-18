using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // TODO: Remove this once heatmap data is collected
using UnityEngine.UI; // TODO: Remove this once heatmap data is collected
using Zenject;
using TMPro;
using UnityEngine.Analytics;
using Yarn.Unity;
using Rewired;

[System.Serializable]
public class GameUIEvent : UnityEvent<GameUIController> { }

public class GameUIController : MonoBehaviour
{
    [Header("Dialogue System")]
    [SerializeField] private DialogueRunner dialogueRunner;

    [Header("Upgrade Window")]
    [SerializeField] private GameObject upgradeWindow;

    [Header("Pause Menu")]
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Settings Menu")]
    [SerializeField] private SettingsMenuController settingsMenu;
    [SerializeField] private GameObject controlMapperWindow;

    [Header("Notification System")]
    [SerializeField] private NotificationQueue notificationQueue;
    [SerializeField] private NotificationWindow notificationWindow;
    [SerializeField] private GameObject notificationLabel;

    [Header("Objectives System")]
    [SerializeField] private ObjectivesPanel objectivesPanel;

    [Header("Subtitle System")]
    [SerializeField] private CanvasGroup subtitlePanel;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private AudioSource voiceAudioSource;

    [Header("Music Player")]
    [SerializeField] private AudioSource musicPlayerSource;

    [Header("HUD Elements")]
    [SerializeField] private EntityHealthBar playerHealthBar;
    [SerializeField] private GameObject playerEnergyBar;
    [SerializeField] private GameObject gameOverPanel; // TODO: Remove this once heatmap data is collected
    [SerializeField] private Button restartGameButton; // TODO: Remove this once heatmap data is collected

    [Inject] private HeatmapUploadController heatmap;
    [Inject] private IInputController input;
    [Inject] private SettingsManager settings;
    [Inject] private MusicPlayer musicPlayer;

    private float elapsedTime;
    private float refreshTime = 3.0f;

    private bool paused = false;
    private bool notificationWindowActive = false;
    private bool isTalking = false;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);

        elapsedTime = 0.0f;

        settings.UpdateFont();

        ReInput.ControllerConnectedEvent += UpdateButtonUI;
        ReInput.ControllerDisconnectedEvent += UpdateButtonUI;

        //PushFirstNotification();
        //PushTestNotification();
    }

    // Update is called once per frame
    void Update()
    {            
        CheckIfPaused();
        CheckNotificationWindow();
        CheckObjectivesPanel();

        if (elapsedTime > refreshTime) {
            elapsedTime = 0.0f;

            if (heatmap.gameObject.activeInHierarchy)
                heatmap.SaveLocationsToFile();
        }

        elapsedTime += Time.deltaTime;
    }

    public void SetCamera(Camera camera)
    {
        GetComponent<Canvas>().worldCamera = camera;
    }

    public void SetControlMapper(GameObject mapper)
    {
        controlMapperWindow = mapper;
        controlMapperWindow.SetActive(false);
    }

    public ObjectivesPanel GetObjectivesPanel()
    {
        return objectivesPanel;
    }

    public bool IsTalking()
    {
        return isTalking;
    }

    public void StartConversation(string startNode)
    {
        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        HideHUD();
        dialogueRunner.StartDialogue(startNode);
        isTalking = true;
    }

    public void FinishConversation()
    {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isTalking = false;
        ShowHUD();
    }

    public void HideHUD()
    {
        objectivesPanel.Hide();
        notificationQueue.gameObject.SetActive(false);
        notificationLabel.gameObject.SetActive(false);
        playerHealthBar.Hide();
        playerEnergyBar.SetActive(false);
    }

    public void ShowHUD()
    {
        objectivesPanel.Show();
        notificationQueue.gameObject.SetActive(true);
        notificationLabel.gameObject.SetActive(true);
        playerHealthBar.Show();
        playerEnergyBar.SetActive(true);
    }

    [YarnCommand("ShowUpgradeWindow")]
    public void ShowUpgradeWindow()
    {
        upgradeWindow.SetActive(true);
    }

    private void CheckIfPaused()
    {
        if (input.Pause()) {

            if (!paused) {
                PauseGame();

                // Open menu
                pauseMenu.Show();
                HideHUD();
            } else {
                if (notificationWindowActive)
                    return;

                StopPause();
            }
        }
    }

    public bool IsInteractingWithUI()
    {
        return paused || IsTalking();
    }

    public void StopPause()
    {
        pauseMenu.Hide();
        ShowHUD();
        UnpauseGame();
    }

    public void SwapToSettingsMenu()
    {
        settingsMenu.gameObject.SetActive(true);
        pauseMenu.Hide();
    }

    public void ReturnToPauseMenu()
    {
        settingsMenu.gameObject.SetActive(false);
        pauseMenu.Show();
    }

    public void ShowControlMapper()
    {
        settingsMenu.gameObject.SetActive(false);
        controlMapperWindow.SetActive(true);
    }

    public void HideControlMapper()
    {
        controlMapperWindow.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }

    // TODO: Remove once heatmap data is collected
    public void ShowGameOverPanel()
    {
        PauseGame();
        HideHUD();
        gameOverPanel.SetActive(true);

        restartGameButton.Select();
        restartGameButton.OnSelect(null);

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level_index", 1);

        //AnalyticsResult result = AnalyticsEvent.Custom("level_complete", parameters);
        //if (result == AnalyticsResult.Ok) {
        //    Logger.Debug("All is well!");
        //} else {
        //    Logger.Error("We have a problem with the Analytics data");
        //}
    }

    // TODO: Remove once heatmap data is collected
    public void RestartGame()
    {
        UnpauseGame();
        SceneManager.LoadScene("TutorialLevel");
    }
    
    private void PauseGame()
    {
        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //inputModule.inputActionsPerSecond = 1;

        musicPlayer.PauseAllAudio();

        // Pause time
        Time.timeScale = 0.0f;

        paused = true;
    }

    private void UnpauseGame()
    {
        settings.UpdateFont();

        // Fix volume settings
        voiceAudioSource.volume = Mathf.Clamp01(settings.GetVoiceVolume());
        musicPlayerSource.volume = Mathf.Clamp01(settings.GetMusicVolume());

        musicPlayer.UnpauseAllAudio();

        // Unpause the game
        paused = false;
        Time.timeScale = settings.CurrentGameSpeed() / 100f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CheckNotificationWindow()
    {
        bool open = input.OpenNotificationWindow();

        // Check if already paused
        if (paused)
            return;

        // If window is already open, return
        if (notificationWindow.gameObject.activeSelf)
            return;

        if (open) {
            PauseGame();
            notificationWindowActive = true;
            notificationWindow.gameObject.SetActive(true);
            HideHUD();
        }
    }

    public void HideNotificationWindow()
    {
        ShowHUD();
        notificationWindow.gameObject.SetActive(false);
        UnpauseGame();
    }

    public void CheckObjectivesPanel()
    {
        bool toggled = input.ToggleObjectivesPanel();

        // Check if already paused
        if (paused)
            return;

        // If window is already open, return
        if (notificationWindow.gameObject.activeSelf)
            return;

        if (toggled)
        {
            objectivesPanel.TogglePanel();
        }
    }

    public void PushTestNotification()
    {
        notificationQueue.Push("Test Notification");
    }

    public void PushNotification(Notification n)
    {
        notificationQueue.Push(n);
    }

    public void ShowSubtitles(SubtitleClip clip)
    {
        StartCoroutine(FadeInSubtitles(clip));
    }

    public void HideSubtitles(SubtitleClip clip)
    {
        if (subtitlePanel.alpha == 1)
            StartCoroutine(FadeAfterAudio(clip));
    }

    public EntityHealthBar GetHealthBar()
    {
        return playerHealthBar;
    }

    public void FadeOutElementsForCombat()
    {
        objectivesPanel.Hide();

        CanvasGroup notificationsCG = notificationQueue.GetComponent<CanvasGroup>();

        if (notificationsCG.alpha != 0) {
            StartCoroutine(FadeCanvasGroup(notificationsCG, 100, 0));
        }
    }

    public void FadeInElementsAfterCombat()
    {
        objectivesPanel.Show();

        CanvasGroup notificationsCG = notificationQueue.GetComponent<CanvasGroup>();

        if (notificationsCG.alpha != 100) {
            StartCoroutine(FadeCanvasGroup(notificationsCG, 0, 100));
        }
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true) {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }

        //Destroy(this.gameObject);
    }

    private IEnumerator FadeInSubtitles(SubtitleClip clip)
    {
        subtitleText.text = clip.Subtitles;
        if (subtitlePanel.alpha != 1) {
            StartCoroutine(FadeCanvasGroup(subtitlePanel, 0, 1));
        }
        yield return new WaitForSeconds(1);
        // This is working
        voiceAudioSource.clip = clip.Audio;
        voiceAudioSource.Play();

        yield return new WaitForEndOfFrame();
        HideSubtitles(clip);
    }

    private IEnumerator FadeAfterAudio(SubtitleClip clip)
    {
        yield return new WaitForSeconds(clip.GetDuration()); // Number of words * 0.4 = clip duration

        StartCoroutine(FadeCanvasGroup(subtitlePanel, 1, 0));

        yield return new WaitForSeconds(1); // Make sure that there is a gap between subtitles
    }

    private void UpdateButtonUI(ControllerStatusChangedEventArgs args)
    {
        ShowControllerButtonUI[] buttonPrompts = GameObject.FindObjectsOfType<ShowControllerButtonUI>();

        foreach (ShowControllerButtonUI buttonPrompt in buttonPrompts)
        {
            buttonPrompt.UpdateImage();
        }
    }
}
