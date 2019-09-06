using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HeatmapController : MonoBehaviour
{
    private List<Vector2> positionList;

    private string filePath = "";
    private string url = "https://killerbyteworkshop.com/heatmapupload.php";

    public void AddPosition(Vector2 position)
    {
        positionList.Add(position);
    }

    public void SendFilesToServer()
    {

    }

    IEnumerator WWWRequest()
    {
        UnityWebRequest file = new UnityWebRequest();
        WWWForm form = new WWWForm();

        file = UnityWebRequest.Get(filePath);
        yield return file.SendWebRequest();
        form.AddBinaryData("file", file.downloadHandler.data, Path.GetFileName(filePath));

        UnityWebRequest req = UnityWebRequest.Post(url, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError) {
            Logger.Debug(req.error);
        } else {
            Logger.Debug("Uploaded successfully");
        }

        yield return false;
    }
}
