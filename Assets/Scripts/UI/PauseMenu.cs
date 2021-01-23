using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [Inject] private SceneController _sceneController;
    [Inject] private SettingsManager _settingsManager;
    [Inject] private MusicPlayer musicPlayer;

    [SerializeField] private Button resumeGameButton;

    private void OnEnable()
    {
        resumeGameButton.Select();
        resumeGameButton.OnSelect(null);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _settingsManager.UpdateFont();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        _sceneController.LoadMainMenu();
    }

    public void QuitGame()
    {
        musicPlayer.Destroy();

//#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
//        Logger.Debug(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
//#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#endif
    }
}
