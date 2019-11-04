using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using TMPro;

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

    [Header("Subtitle System")]
    [SerializeField] private CanvasGroup subtitlePanel;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private AudioSource voiceAudioSource;

    [Header("Music Player")]
    [SerializeField] private AudioSource musicPlayerSource;

    [Header("HUD Elements")]
    [SerializeField] private GameObject gameplayHUD;

    [Inject] private HeatmapUploadController heatmap;
    [Inject] private IInputController inputController;
    [Inject] private SettingsManager settingsManager;

    private float elapsedTime;
    private float refreshTime = 3.0f;

    private bool paused = false;

    public static GameUIController Instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        controlMapperWindow.SetActive(false);

        elapsedTime = 0.0f;

        Instance = this;

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

    public void StartConversation(Conversation conversation)
    {
        conversationController.ChangeConversation(conversation);
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
        if (inputController.Pause()) {

            if (!paused) {
                PauseGame();

                // Open menu
                pauseMenu.Show();
                gameplayHUD.SetActive(false);
            } else {
                StopPause();
                gameplayHUD.SetActive(true);
            }
        }
    }

    public bool IsInteractingWithUI()
    {
        return paused || IsConversationActive();
    }

    public void StopPause()
    {
        UnpauseGame();
        pauseMenu.Hide();
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

    private void PauseGame()
    {
        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Pause time
        Time.timeScale = 0.0f;

        paused = true;
    }

    private void UnpauseGame()
    {
        // Fix volume settings
        voiceAudioSource.volume = Mathf.Clamp01(settingsManager.GetVoiceVolume());
        musicPlayerSource.volume = Mathf.Clamp01(settingsManager.GetMusicVolume());

        // Unpause the game
        paused = false;
        Time.timeScale = settingsManager.CurrentGameSpeed() / 100f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CheckNotificationWindow()
    {
        bool open = inputController.OpenNotificationWindow();

        // If window is already open, return
        if (window.gameObject.activeSelf)
            return;

        if (open) {
            PauseGame();
            window.gameObject.SetActive(true);
        }
    }

    public void HideNotificationWindow()
    {
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
