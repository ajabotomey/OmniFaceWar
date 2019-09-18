using UnityEngine;
using UnityEngine.Events;
using Zenject;

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

    [Inject] private HeatmapUploadController heatmap;
    [Inject] private IInputController inputController;

    private float elapsedTime;
    private float refreshTime = 3.0f;

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        controlMapperWindow.SetActive(false);

        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPaused();

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
                // Show the cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                // Pause time
                Time.timeScale = 0.0f;

                // Open menu
                pauseMenu.Show();

                paused = true;
            } else {
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
        paused = false;
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    //private void OnApplicationQuit()
    //{
    //    heatmap.SendFilesToServer();
    //}
}
