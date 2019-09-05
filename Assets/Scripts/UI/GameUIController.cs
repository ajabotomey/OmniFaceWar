using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameUIEvent : UnityEvent<GameUIController> { }

public class GameUIController : MonoBehaviour
{
    [SerializeField] private ConversationController conversationController;
    [SerializeField] private GameObject upgradeWindow;

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

    public void ShowUpgradeWindow()
    {
        upgradeWindow.SetActive(true);
        conversationController.EndConversation();
    }
}
