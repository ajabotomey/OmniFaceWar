using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tweet : MonoBehaviour
{
    private string id;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI tweetText;

    public void SetTweet(string id, string name, string tweet)
    {
        this.id = id;
        nameText.text = name;
        tweetText.text = tweet;
    }

    public void SetTweet(string id, string tweet)
    {
        this.id = id;
        tweetText.text = tweet;
    }

    public void OnClick()
    {
        Application.OpenURL("https://twitter.com/KillerbyteWShop/status/" + id);
    }
}
