using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // TODO: Remove this once heatmap data is collected
using UnityEngine.UI; // TODO: Remove this once heatmap data is collected
using Zenject;
using TMPro;
using UnityEngine.Analytics;

[System.Serializable]
public class GameUIEvent : UnityEvent<GameUIController> { }

public class GameUIController : MonoBehaviour
{
    [Header("Dialogue System")]
    [SerializeField] private ConversationController conversationController;

    [Header("Upgrade Window")]
    [SerializeField] private GameObject upgradeWindow;

    [Header("Pause Menu")]
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Settings Menu")]
    [SerializeField] private SettingsMenuController settingsMenu;
    [SerializeField] private GameObject controlMapperWindow;

    [Header("Notification System")]
    [SerializeField] private NotificationQueue queue;
    [SerializeField] private NotificationWindow window;

    [Header("Objectives System")]
    [SerializeField] private ObjectivesPanel objectivesPanel;

    [Header("Subtitle System")]
    [SerializeField] private CanvasGroup subtitlePanel;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private AudioSource voiceAudioSource;

    [Header("Music Player")]
    [SerializeField] private AudioSource musicPlayerSource;

    [Header("HUD Elements")]
    [SerializeField] private GameObject gameplayHUD;
    [SerializeField] private EntityHealthBar playerHealthBar;
    [SerializeField] private GameObject gameOverPanel; // TODO: Remove this once heatmap data is collected
    [SerializeField] private Button restartGameButton; // TODO: Remove this once heatmap data is collected

    [Inject] private HeatmapUploadController heatmap;
    [Inject] private IInputController input;
    [Inject] private SettingsManager settings;

    private float elapsedTime;
    private float refreshTime = 3.0f;

    private bool paused = false;
    private bool notificationWindowActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);

        elapsedTime = 0.0f;

        settings.UpdateFont();

        //PushFirstNotification();
        //PushTestNotification();
    }

    // Update is called once per frame
    void Update()
    {            
        CheckIfPaused();
        CheckNotificationWindow();

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

    public void StartConversation(Conversation conversation)
    {
        conversationController.ChangeConversation(conversation);
        conversationController.gameObject.SetActive(true);
    }

    public bool IsConversationActive()
    {
        return conversationController.IsConversationActive();
    }

    public void ShowUpgradeWindow()
    {
        upgradeWindow.SetActive(true);
        conversationController.EndConversation();
    }

    private void CheckIfPaused()
    {
        if (input.Pause()) {

            if (!paused) {
                PauseGame();

                // Open menu
                pauseMenu.Show();
                gameplayHUD.SetActive(false);
            } else {
                if (notificationWindowActive)
                    return;

                StopPause();
            }
        }
    }

    public bool IsInteractingWithUI()
    {
        return paused || IsConversationActive();
    }

    public void StopPause()
    {
        pauseMenu.Hide();
        gameplayHUD.SetActive(true);
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
        gameplayHUD.SetActive(false);
        gameOverPanel.SetActive(true);

        restartGameButton.Select();
        restartGameButton.OnSelect(null);

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level_index", 1);

        AnalyticsResult result = AnalyticsEvent.Custom("level_complete", parameters);
        if (result == AnalyticsResult.Ok) {
            Logger.Debug("All is well!");
        } else {
            Logger.Error("We have a problem with the Analytics data");
        }
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
        if (window.gameObject.activeSelf)
            return;

        if (open) {
            PauseGame();
            notificationWindowActive = true;
            window.gameObject.SetActive(true);
            gameplayHUD.SetActive(false);
        }
    }

    public void HideNotificationWindow()
    {
        gameplayHUD.SetActive(true);
        window.gameObject.SetActive(false);
        UnpauseGame();
    }

    public void PushTestNotification()
    {
        queue.Push("Test Notification");
    }

    public void PushNotification(Notification n)
    {
        queue.Push(n);
    }

    public void ShowSubtitles(SubtitleClip clip)
    {
        StartCoroutine(FadeInSubtitles(clip));
    }

    public void HideSubtitles()
    {
        if (subtitlePanel.alpha == 1)
            StartCoroutine(FadeAfterAudio());
    }

    public EntityHealthBar GetHealthBar()
    {
        return playerHealthBar;
    }

    public void FadeOutElementsForCombat()
    {
        CanvasGroup objectivesCG = objectivesPanel.GetComponent<CanvasGroup>();
        CanvasGroup notificationsCG = queue.GetComponent<CanvasGroup>();

        if (objectivesCG.alpha != 0 && notificationsCG.alpha != 0) {
            StartCoroutine(FadeCanvasGroup(objectivesCG, 100, 0));
            StartCoroutine(FadeCanvasGroup(notificationsCG, 100, 0));
        }
    }

    public void FadeInElementsAfterCombat()
    {
        CanvasGroup objectivesCG = objectivesPanel.GetComponent<CanvasGroup>();
        CanvasGroup notificationsCG = queue.GetComponent<CanvasGroup>();

        if (objectivesCG.alpha != 100 && notificationsCG.alpha != 100) {
            StartCoroutine(FadeCanvasGroup(objectivesCG, 0, 100));
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
    }

    private IEnumerator FadeAfterAudio()
    {
        while (voiceAudioSource.isPlaying) {
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(FadeCanvasGroup(subtitlePanel, 1, 0));

        yield return new WaitForEndOfFrame();
    }
}
