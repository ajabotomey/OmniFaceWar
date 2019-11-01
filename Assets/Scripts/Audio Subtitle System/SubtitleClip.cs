using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Subtitle Clip", menuName = "Audio Subtitle System/New Subtitle Clip")]
public class SubtitleClip : ScriptableObject
{
    [SerializeField] private AudioClip audio;
    [SerializeField] [TextArea] private string subtitles;

    public AudioClip Audio {
        get { return audio; }
    }

    public string Subtitles {
        get { return subtitles; }
    }
}
