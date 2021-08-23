using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Tweet : Selectable, IPointerClickHandler, ISubmitHandler
{
    private string id;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI tweetText;
    [SerializeField] private VoidEvent tweetSelected;
    [SerializeField] private Button button;

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

    public override void OnSelect(BaseEventData eventData)
    {
        tweetSelected.Raise();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnClick();
    }
}
