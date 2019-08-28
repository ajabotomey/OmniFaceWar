using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Line
{
    public Character character;

    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue System/New Conversation")]
public class Conversation : ScriptableObject
{
    [SerializeField] private Character[] characters;

    [SerializeField] private Line[] lines;
    [SerializeField] private Question question;
    [SerializeField] private Conversation nextConversation;



    public Line GetLine(int index)
    {
        return lines[index];
    }

    public int GetConversationLength()
    {
        return lines.Length;
    }

    public Question GetQuestion()
    {
        return question;
    }

    public Conversation GetNextConversation()
    {
        return nextConversation;
    }
}
