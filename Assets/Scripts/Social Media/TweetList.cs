using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class TweetList : MonoBehaviour
{
    private string tweetListURL = "https://api.twitter.com/2/users/1161459229766500354/tweets?max_results=50&expansions=author_id,referenced_tweets.id"; // Get Timeline for Killerbyte Workshop
    [SerializeField] private string bearerToken = "";

    [Header("Tweet Prefab")]
    [SerializeField] private Tweet tweetPrefab;
    public GameObject CurrentlySelectedTweet { get; private set; }

    [Header("Scroll")]
    [SerializeField] private RectTransform scrollRect;
    [SerializeField] private Scrollbar scrollbar;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI internetText;

    [Header("Event")]
    [SerializeField] private VoidEvent tweetsUpdated;

    private string data = "";
    private List<TweetData> tweets;

    public bool TweetsActive { get; private set;} = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTweetsFromTwitterAPI());
    }

    void Update()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    IEnumerator GetTweetsFromTwitterAPI()
    {
        Logger.Debug("Retreving Tweets from the Twitter API now");

        UnityWebRequest req = new UnityWebRequest(tweetListURL);
        req.method = UnityWebRequest.kHttpVerbGET;
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Authorization", "Bearer " + bearerToken);

        yield return req.SendWebRequest();

        Debug.Log("Response Code: " + req.responseCode);
        Debug.Log("Response: " + req.downloadHandler.text);
        data = req.downloadHandler.text;
        
        if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError) {
            Logger.Error("We have a problem: " + req.error);
            internetText.gameObject.SetActive(true);
            TweetsActive = false;
        } else {
            Logger.Debug("Twitter API Requested Successfully");
            ProcessTweetData();
        }
    }

    private void ProcessTweetData()
    {
        tweets = new List<TweetData>();

        JSONReader.ReadTwitterList(ref tweets, ref data);

        PopulateTweetList();
    }

    private void PopulateTweetList()
    {
        foreach (TweetData tweet in tweets) {
            GameObject newTweet = Instantiate(tweetPrefab.gameObject);
            RectTransform rect = newTweet.GetComponent<RectTransform>();
            newTweet.GetComponent<Tweet>().SetTweet(tweet.id, tweet.text);
            rect.SetParent(scrollRect);
            rect.localScale = Vector3.one;

            if (tweet == tweets[0]) {
                CurrentlySelectedTweet = newTweet;
            }
        }

        TweetsActive = true;
        tweetsUpdated.Raise();
    }

    public void OnTweetSelected()
    {
        CurrentlySelectedTweet = EventSystem.current.currentSelectedGameObject;
    }
}

[System.Serializable]
public class TweetData {
    public string id;
    public string text;
}
