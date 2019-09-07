using UnityEngine;
using UnityEngine.Events;
using Zenject;

[System.Serializable]
public class GameUIEvent : UnityEvent<GameUIController> { }

public class GameUIController : MonoBehaviour
{
    [SerializeField] private ConversationController conversationController;
    [SerializeField] private GameObject upgradeWindow;

    [Inject] private HeatmapUploadController heatmap;

    private float elapsedTime;
    private float refreshTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > refreshTime) {
            elapsedTime = 0.0f;
            heatmap.SaveLocationsToFile();
        }

        elapsedTime += Time.deltaTime;
    }

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

    //private void OnApplicationQuit()
    //{
    //    heatmap.SendFilesToServer();
    //}
}
