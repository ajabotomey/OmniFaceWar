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
using UnityEngine.InputSystem;

[System.Serializable]
public class GameUIEvent : UnityEvent<GameUIController> { }

public class GameUIController : MonoBehaviour
{
    [Header("Device Display")]
    [SerializeField] private DeviceDisplayConfigurator deviceDisplay;

    [Header("Dialogue System")]
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Upgrade Window")]
    [SerializeField] private GameObject upgradeWindow;

    [Header("Pause Menu")]
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Save Menu")]
    [SerializeField] private GameObject saveMenu;

    [Header("Settings Menu")]
    [SerializeField] private SettingsMenuController settingsMenu;

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
    [SerializeField] private GameObject playerPlateObj;
    [SerializeField] private WeaponWheelController weaponWheel;
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

    private bool objectivesPanelToggled = false;

    // Start is called before the first frame update
    void Awake()
    {
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);

        elapsedTime = 0.0f;

        settings.UpdateFont();
    }

    // Update is called once per frame
    void Update()
    {            
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
        if (isTalking)
            return;

        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        HideHUD();
        settings.UpdateFont();
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

    public void SelectContinueButtonAfterDialogue(Button button) 
    {
        button.gameObject.SetActive(true);

        button.Select();
        button.OnSelect(null);
    }

    public void HideHUD()
    {
        objectivesPanel.Hide();
        notificationQueue.gameObject.SetActive(false);
        notificationLabel.gameObject.SetActive(false);
        //playerHealthBar.Hide();
        playerPlateObj.SetActive(false);
    }

    public void ShowHUD()
    {
        objectivesPanel.Show();
        notificationQueue.gameObject.SetActive(true);
        notificationLabel.gameObject.SetActive(true);
        //playerHealthBar.Show();
        playerPlateObj.SetActive(true);
    }

    [YarnCommand("ShowUpgradeWindow")]
    public void ShowUpgradeWindow()
    {
        upgradeWindow.SetActive(true);
    }

    public void CheckIfPaused(InputAction.CallbackContext value)
    {
        if (value.started) {

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

    public void SaveGame()
    {
        saveMenu.SetActive(true);
    }

    public void ReturnFromSaveGame()
    {
        saveMenu.SetActive(false);
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

    public void CheckNotificationWindow(InputAction.CallbackContext value)
    {
        // Check if already paused
        if (paused)
            return;

        // If window is already open, return
        if (notificationWindow.gameObject.activeSelf)
            return;

        if (value.started) {
            PauseGame();
            notificationWindowActive = true;
            notificationWindow.gameObject.SetActive(true);
            HideHUD();
        } else {
            HideNotificationWindow();
        }
    }

    public void HideNotificationWindow()
    {
        ShowHUD();
        notificationWindow.gameObject.SetActive(false);
        UnpauseGame();
    }

    public void ToggleObjectivesPanel(InputAction.CallbackContext value)
    {
        if (value.started) {
            // Check if already paused
            if (paused)
                return;

            // If window is already open, return
            if (notificationWindow.gameObject.activeSelf)
                return;

            objectivesPanel.TogglePanel();
        }
    }

    public void HandleWeaponWheel(InputAction.CallbackContext value)
    {
        if (value.started) {

            bool isActive = weaponWheel.isActiveAndEnabled;
            if (isActive) {
                weaponWheel.HandleWeaponWheel(false);
                ShowHUD();
                UnpauseGame();
            } else {
                PauseGame();
                HideHUD();
                weaponWheel.gameObject.SetActive(true);
                weaponWheel.HandleWeaponWheel(true);
            }
        }
    }

    public void HandleWeaponWheelControllerInput(InputAction.CallbackContext value)
    {
        var input = value.ReadValue<Vector2>();
        input.x = Mathf.Round(input.x);
        input.y = Mathf.Round(input.y);
        weaponWheel.ControllerSelect(input);
    }

    public void CancelPauseAndSettings(InputAction.CallbackContext value)
    {
        // TODO: Add in the keybind window
        if (paused && settingsMenu.gameObject.activeInHierarchy == false && settingsMenu.IsInKeyRebind() == false && value.performed)
            StopPause();
        else if (paused && settingsMenu.gameObject.activeInHierarchy == true && settingsMenu.IsInKeyRebind() == false && value.performed)
            ReturnToPauseMenu();
        else if (paused && settingsMenu.gameObject.activeInHierarchy == true && settingsMenu.IsInKeyRebind() == true && value.performed)
            settingsMenu.ReturnFromKeyRebind();
    }

    public void ContinueConversation(InputAction.CallbackContext value)
    {
        if (isTalking)
            dialogueUI.MarkLineComplete();
    }

    public void CheckObjectivesPanel()
    {

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
}
