using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private ConversationController conversationController;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void StartConversation(Conversation conversation)
    {
        conversationController.ChangeConversation(conversation);
    }

    public bool IsConversationActive()
    {
        return conversationController.IsConversationActive();
    }
}
