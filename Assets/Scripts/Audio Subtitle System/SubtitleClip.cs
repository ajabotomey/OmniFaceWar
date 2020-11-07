using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Subtitle Clip", menuName = "Audio Subtitle System/New Subtitle Clip")]
public class SubtitleClip : ScriptableObject
{
    [SerializeField] private AudioClip audio;
    [SerializeField] [TextArea] private string subtitles;
    [SerializeField] private bool isDirectionalSubtitle;

    [Inject] private PlayerControl player;

    public AudioClip Audio {
        get { return audio; }
    }

    public string Subtitles {
        get { return subtitles; }
    }

    public string GetDirectionalSubtitle()
    {


        return subtitles;
    }
}
