using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using UniRx;

[System.Serializable]
public class SceneController
{
    [SerializeField] private string mainMenu;
    [SerializeField] private string[] gameScenes;
    private int currentSceneIndex; // 0 for main menu, 1 for test level

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private Text progressText;

    public void LoadGame()
    {
        //SceneManager.LoadScene(gameScenes[0]);
        //_levelProcessor.StartCoroutine(LoadLevel(gameScenes[0]));
        //var loadScene = Observable.FromCoroutine(LoadLevel, ));
        SceneManager.LoadScene("TutorialLevel");
        
        //currentSceneIndex = 1;
    }

    /*
     * Method for loading game with save file here
     * 
     */

    public void LoadMainMenu()
    {
        //_levelProcessor.StartCoroutine(LoadLevel(mainMenu));
        SceneManager.LoadScene("MainMenu");
        currentSceneIndex = 0;
    }

    public bool IsInGame()
    {
        return currentSceneIndex != 0;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadSceneAsync(levelName).AsAsyncOperationObservable().Do(
            x => {
                // Show loading screen
                //Debug.Log("Progress: " + x.progress);
                float progress = Mathf.Clamp01(x.progress / .9f);

                slider.value = progress;
                progressText.text = progress * 100f + "%";
            }).Subscribe(_ => {
                //Debug.Log("Loaded!");
                // Hide loading screen
                canvas.worldCamera = Camera.main;
            });
    }

    // Add loading screen here
    //IEnumerator LoadLevel(string levelName)
    //{
    //    //enabled = false;
    //    //yield return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
    //    //SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));

    //    AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

    //    //loadingScreen.SetActive(true);

    //    while (!operation.isDone) {
    //    //    float progress = Mathf.Clamp01(operation.progress / .9f);

    //    //    slider.value = progress;
    //    //    progressText.text = progress * 100f + "%";

    //        yield return null;
    //    }

    //    ////enabled = true;

    //    //loadingScreen.SetActive(false);

    //    // Reassign the camera
    //    canvas.worldCamera = Camera.main;
    //}
}
