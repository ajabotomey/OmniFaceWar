using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    //private ISceneController _sceneController;

    [Inject] private SettingsManager settingsManager;
    [Inject] private SceneController sceneController;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitGameButton;

    [SerializeField] private VoidEvent settingsMenuEvent;

    void OnEnable()
    {
        settingsManager.UpdateFont();
        startGameButton.Select();
        startGameButton.OnSelect(null);
    }

    public void StartGame()
    {
        sceneController.LoadGame();
    }

    public void LoadGame()
    {

    }

    public void SettingsMenu()
    {
        settingsMenuEvent.Raise();
    }

    public void QuitGame()
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Logger.Debug(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#endif
    }
}
