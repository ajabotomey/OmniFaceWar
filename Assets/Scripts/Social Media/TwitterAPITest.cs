using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TwitterAPITest : MonoBehaviour
{
    private string uploadURL;
    [SerializeField] private string bearerToken = "";

    [Header("Tweet Prefab")]
    [SerializeField] private GameObject tweetPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTweetsFromTwitterRESTAPI());
    }

    IEnumerator GetTweetsFromTwitterRESTAPI()
    {
        Logger.Debug("Uploading to server now");

        uploadURL = "https://api.twitter.com/2/users/1161459229766500354/tweets?max_results=5&expansions=author_id,referenced_tweets.id";
        Debug.Log(uploadURL);

        UnityWebRequest req = new UnityWebRequest(uploadURL);
        req.method = UnityWebRequest.kHttpVerbGET;
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Authorization", "Bearer " + bearerToken);

        yield return req.SendWebRequest();

        Debug.Log("Response Code: " + req.responseCode);
        Debug.Log("Response: " + req.downloadHandler.text);
        
        if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError) {
            Logger.Error("We have a problem: " + req.error);
        } else {
            Logger.Debug("Twitter API Requested Successfully");
        }
    }
}