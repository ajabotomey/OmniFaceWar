using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

[ExecuteInEditMode]
public class HeatmapDownloadController : MonoBehaviour
{
    private static string downloadURL = "https://killerbyteworkshop.com/heatmapdownload.php";
    private static UnityWebRequest www;

    private static void Request()
    {
        Logger.Debug("Sending request now!");
        www = UnityWebRequest.Get(downloadURL);
        www.Send();
    }

#if UNITY_EDITOR
    private static void EditorUpdate()
    {
        if (!www.isDone)
            return;

        if (www.isNetworkError || www.isHttpError)
            Logger.Error(www.error);
        else {
            Logger.Debug(www.downloadHandler.text);
        }

        EditorApplication.update -= EditorUpdate;
    }

    public void RetrieveData()
    {
        Request();
        EditorApplication.update += EditorUpdate;
    }

#endif

    public string GetData()
    {
        if (www == null) {
            Logger.Debug("Need to retrieve data from server first");
            return "";
        }

        if (www.isNetworkError || www.isHttpError)
            return "";

        return www.downloadHandler.text;
    }
}
