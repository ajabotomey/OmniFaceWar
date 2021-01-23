using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MusicPlayer
{
    private FMOD.Studio.EventInstance instance;

    [FMODUnity.EventRef] [SerializeField] private string levelOneMusic;

    private FMOD.Studio.Bus masterBus; // Path bus:/Master
    private FMOD.Studio.Bus musicBus; // Path: bus:/Music
    private FMOD.Studio.Bus sfxBus; // Path: bus:/SFX
    private FMOD.Studio.Bus voiceBus; // Path: bus:/Voice

    // Start is called before the first frame update
    public void Initialize()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.Equals("TutorialLevel"))
            instance = FMODUnity.RuntimeManager.CreateInstance(levelOneMusic);

        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        voiceBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Voice");

        instance.start();
    }

    public void Destroy()
    {
        instance.release();
    }

    public void PauseAllAudio()
    {
        masterBus.setPaused(true);
        musicBus.setPaused(true);
        sfxBus.setPaused(true);
        voiceBus.setPaused(true);
    }

    public void UnpauseAllAudio()
    {
        masterBus.setPaused(false);
        musicBus.setPaused(false);
        sfxBus.setPaused(false);
        voiceBus.setPaused(false);
    }
}
