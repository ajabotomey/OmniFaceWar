using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Choice
{
    [TextArea(2, 5)] public string text;
    public Conversation conversation;
}

[CreateAssetMenu(fileName = "New Question", menuName = "Dialogue System/New Question")]
public class Question : ScriptableObject
{
    [TextArea(2, 5)] [SerializeField] private string text;
    [SerializeField] private Choice[] choices;

    public string GetText()
    {
        return text;
    }

    public int GetChoiceLength()
    {
        return choices.Length;
    }

    public Choice GetChoice(int index)
    {
        return choices[index];
    }
}
