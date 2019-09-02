using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestNPC : MonoBehaviour
{
    [SerializeField] private Conversation conversation;

    [Inject]
    private IInputController inputController;
    [Inject]
    private GameUIController gameUIController;

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    // If a button is pushed, start conversation
    //    if (inputController.TalkToNPC()) {
    //        Logger.Debug("Talk to NPC!");
    //    }
    //}

    private bool canTalk = false;
    private bool isTalking = false;

    private void Update()
    {
        if (canTalk && inputController.TalkToNPC() && !isTalking) {
            Logger.Debug("Talk to NPC!");

            gameUIController.StartConversation(conversation);
        }

        isTalking = gameUIController.IsConversationActive();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canTalk = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canTalk = false;
    }
}
