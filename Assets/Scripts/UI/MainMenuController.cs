using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    //private ISceneController _sceneController;

    [Inject] private SettingsManager settingsManager;
    [Inject] private SceneController sceneController;

    [Header("Buttons")]
    [SerializeField] private Selectable startGameButton;
    [SerializeField] private Selectable loadGameButton;
    [SerializeField] private Selectable settingsButton;
    [SerializeField] private Selectable quitGameButton;

    [Header("Tweet List")]
    [SerializeField] private TweetList tweetList;

    [Header("Events")]
    [SerializeField] private VoidEvent settingsMenuEvent;

    void OnEnable()
    {
        settingsManager.UpdateFont();
        startGameButton.Select();
        //startGameButton.OnSelect(null);

        //UpdateNavigation();
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
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#endif
    }

    public void SelectPlayButton()
    {
        startGameButton.Select();
        startGameButton.OnSelect(null);
    }

    public void UpdateNavigation()
    {
        // Update the navigation to the currently selected tweet
        // First tweet in the list should be the default

        Navigation startGameNav = startGameButton.navigation;
        Navigation loadGameNav = loadGameButton.navigation;
        Navigation settingsNav = settingsButton.navigation;
        Navigation quitGameNav = quitGameButton.navigation;

        startGameNav.selectOnRight = tweetList.CurrentlySelectedTweet.GetComponent<Tweet>();
        loadGameNav.selectOnRight = tweetList.CurrentlySelectedTweet.GetComponent<Tweet>();
        settingsNav.selectOnRight = tweetList.CurrentlySelectedTweet.GetComponent<Tweet>();
        quitGameNav.selectOnRight = tweetList.CurrentlySelectedTweet.GetComponent<Tweet>();

        startGameButton.navigation = startGameNav;
        loadGameButton.navigation = loadGameNav;
        settingsButton.navigation = settingsNav;
        quitGameButton.navigation = quitGameNav;
    }
}
