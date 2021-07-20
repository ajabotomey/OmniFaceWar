using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LitJson;

public class HeatmapUploadController : MonoBehaviour
{
    private List<Vector2> positionList = new List<Vector2>();

    private string filePath = "";
    private string uploadURL = "";
    
    private string fileName = "";

    public void AddPosition(Vector2 position)
    {
        positionList.Add(position);
    }

    public void SaveLocationsToFile()
    {
        // Take list and write to file
        //WriteListToFile();

        StartCoroutine(SendJsonToServer());
    }

    private string WriteJson()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);

        writer.WriteObjectStart();

        writer.WritePropertyName("positions");
        writer.WriteArrayStart();

        foreach (Vector2 position in positionList) {
            writer.WriteObjectStart();

            writer.WritePropertyName("x");
            writer.Write(position.x);

            writer.WritePropertyName("y");
            writer.Write(position.y);

            writer.WriteObjectEnd();   
        }

        writer.WriteArrayEnd();
        writer.WriteObjectEnd();

        positionList.Clear();

        return sb.ToString();
    }

    IEnumerator SendJsonToServer()
    {
        Logger.Debug("Uploading to server now");

        fileName = DateTime.UtcNow.ToString("yyyyMMdd_hhmmss") + ".json";
        uploadURL = "http://localhost:3000/" + SceneManager.GetActiveScene().name.ToLower() + "/upload/";
        Debug.Log(uploadURL);

        string json = WriteJson();

        UnityWebRequest req = new UnityWebRequest(uploadURL);
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        Debug.Log("Response Code: " + req.responseCode);
        Debug.Log("Response: " + req.downloadHandler.text);

        if (req.result == UnityWebRequest.Result.ProtocolError || req.result == UnityWebRequest.Result.ConnectionError) {
            Logger.Error("We have a problem: " + req.error);
        } else {
            Logger.Debug("Uploaded successfully");
        }
    }
}
