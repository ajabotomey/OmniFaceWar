using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class QuestionEvent : UnityEvent<Question> { }

public class ConversationController : MonoBehaviour
{
    [SerializeField] private Conversation conversation;
    [SerializeField] private QuestionEvent questionEvent;

    [SerializeField] private SpeakerUI speakerUI;

    private int activeLineIndex = 0;
    private bool conversationStarted = false;

    public void ChangeConversation(Conversation nextConversation)
    {
        conversationStarted = false;
        conversation = nextConversation;
        AdvanceLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            AdvanceLine();
        } else if (Input.GetKeyDown("x")) {
            EndConversation();
        }
    }

    private void EndConversation()
    {
        conversation = null;
        conversationStarted = false;
        speakerUI.Hide();
    }

    public bool IsConversationActive()
    {
        return conversationStarted;
    }

    private void Initialize()
    {
        conversationStarted = true;
        activeLineIndex = 0;
        speakerUI.Show();
    }

    private void AdvanceLine()
    {
        if (conversation == null) return;
        if (!conversationStarted) Initialize();

        if (activeLineIndex < conversation.GetConversationLength()) {
            DisplayLine();
        } else {
            AdvanceConversation();
        }
    }

    private void DisplayLine()
    {
        Line line = conversation.GetLine(activeLineIndex);
        Character character = line.character;

        string text = ParseEmojis.Parse(line.text);

        SetDialog(character, text);

        activeLineIndex++;
    }

    private void AdvanceConversation()
    {
        if (conversation.GetQuestion() != null) {
            questionEvent.Invoke(conversation.GetQuestion());

            // Clear the conversation dialog box
            ClearSpeakerUI();
        } else if (conversation.GetNextConversation() != null)
            ChangeConversation(conversation.GetNextConversation());
        else
            EndConversation();
    }

    private void SetDialog(Character character, string text)
    {
        speakerUI.Speaker = character;
        speakerUI.Dialog = text;
    }

    private void ClearSpeakerUI()
    {
        speakerUI.Speaker = null;
        speakerUI.Dialog = "";
    }
}
