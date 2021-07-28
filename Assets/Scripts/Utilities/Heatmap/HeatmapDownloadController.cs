using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEditor;

[ExecuteInEditMode]
public class HeatmapDownloadController : MonoBehaviour
{
    private static UnityWebRequest www;

    private static void Request()
    {
        var downloadURL = "https://gentle-cove-10236.herokuapp.com/" + SceneManager.GetActiveScene().name.ToLower() + "/download/";
        Logger.Debug("Sending request now!");
        www = UnityWebRequest.Get(downloadURL);
        //www.Send();
        www.SendWebRequest();
    }

#if UNITY_EDITOR
    private static void EditorUpdate()
    {
        if (!www.isDone)
            return;

        if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError) 
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

        if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError) 
            return "";

        return www.downloadHandler.text;
    }
}
