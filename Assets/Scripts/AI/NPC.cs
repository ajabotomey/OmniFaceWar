using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    [Header("Yarn Variables")]
    public string characterName = "";
    public string talkToNode = "";

    [Header("Optional")]
    public YarnProject scriptToLoad;

    void Start()
    {
        if (scriptToLoad != null) {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            //dialogueRunner.Add(scriptToLoad);
        }
    }

    public virtual void StartDialogue()
    {

    }
}