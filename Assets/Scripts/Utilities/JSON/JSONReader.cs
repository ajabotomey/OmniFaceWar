using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class JSONReader : MonoBehaviour
{
    [ExecuteInEditMode]
    public static void ReadPositionList(ref Dictionary<Vector2, int> positionList, ref string data, ref int highestCount)
    {
        if (string.IsNullOrEmpty(data)) {
            Logger.Debug("JSON String is null or empty");
        }

        var positionData = JsonMapper.ToObject<JsonData>(data);

        if (JsonDataContainsKey(positionData, "positions")) {
            if (!positionData["positions"].IsArray) {
                Logger.Error("There are no positions");
                return;
            }

            JsonData positions = positionData["positions"];
            foreach (JsonData position in positions) {
                Vector2 pos = new Vector2();
                int count = 0;
                // Parsing X
                if (JsonDataContainsKey(position, "x"))
                    pos.x = float.Parse(position["x"].ToString());
                else {
                    Debug.Log("<color=red>Failed parsing the x coordinate</color>");
                    break;
                }

                // Parsing Y
                if (JsonDataContainsKey(position, "y"))
                    pos.y = float.Parse(position["y"].ToString());
                else {
                    Debug.Log("<color=red>Failed parsing the y coordinate</color>");
                    break;
                }

                // Parsing the count
                if (JsonDataContainsKey(position, "count")){
                    count = int.Parse(position["count"].ToString());

                    if (count > highestCount)
                        highestCount = count;
                }
                else {
                    Debug.Log("<color=red>Failed parsing the count coordinate</color>");
                    break;
                }

                positionList.Add(pos, count);
                Debug.Log("Position: " + pos.ToString() + " was counted " + count + " times");
            }
        }
    }

    public static void ReadTwitterList(ref List<TweetData> tweetList, ref string data)
    {
        if (string.IsNullOrEmpty(data)) {
            Logger.Debug("JSON String is null or empty");
            return;
        }

        var tweetListData = JsonMapper.ToObject<JsonData>(data);

        if (JsonDataContainsKey(tweetListData, "data")) {
            if (!tweetListData["data"].IsArray) {
                Logger.Error("There are no tweets");
                return;
            }

            JsonData tweetData = tweetListData["data"];
            foreach (JsonData tweet in tweetData) {
                TweetData tweetDataObject = new TweetData();

                // If this comes up null, then we know it's a reply and we can skip this
                if (!JsonDataContainsKey(tweet, "author_id")) // TODO: Redo this part to account for retweets and quote tweets. No replies however
                {
                    continue;
                }

                // Parsing the tweet id
                if (JsonDataContainsKey(tweet, "id")) {
                    tweetDataObject.id = tweet["id"].ToString();
                }

                if (JsonDataContainsKey(tweet, "text")) {
                    tweetDataObject.text = tweet["text"].ToString();
                }

                tweetList.Add(tweetDataObject);
            }
        }
    }

    [ExecuteInEditMode]
    private static bool JsonDataContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
            return result;
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key) && tdictionary[key] != null)
            result = true;

        return result;
    }
}
