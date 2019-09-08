using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class HeatmapUploadController : MonoBehaviour
{
    private List<Vector2> positionList = new List<Vector2>();

    private string filePath = "";
    private string uploadURL = "https://killerbyteworkshop.com/heatmapupload.php";
    
    private string fileName = "";

    public void AddPosition(Vector2 position)
    {
        positionList.Add(position);
    }

    public void SaveLocationsToFile()
    {
        // Take list and write to file
        WriteListToFile();

        StartCoroutine(SendFileToServer());
    }

    private void WriteListToFile()
    {
        fileName = DateTime.UtcNow.ToString("yyyyMMdd_hhmmss") + ".txt";
        filePath = Application.dataPath + "/" + fileName;

        //if (!File.Exists(filePath)) {
        //    File.WriteAllText(filePath, "");
        //}

        using (FileStream fs = new FileStream(filePath, FileMode.Create)) {
            using (StreamWriter writer = new StreamWriter(fs)) {
                foreach (Vector2 position in positionList) {
                    writer.WriteLine(position.ToString());
                }

                writer.Close();
            }

            fs.Close();
        }

        positionList.Clear();
    }

    IEnumerator SendFileToServer()
    {
        Logger.Debug("Uploading to server now");

        byte[] txtFile = File.ReadAllBytes(filePath);

        //UnityWebRequest file = new UnityWebRequest();
        WWWForm form = new WWWForm();

        //file = UnityWebRequest.Get(filePath);
        //yield return file.SendWebRequest();
        form.AddBinaryData("userFile", txtFile, fileName);

        UnityWebRequest req = UnityWebRequest.Post(uploadURL, form);
        yield return req.SendWebRequest();

        if (req.isHttpError || req.isNetworkError) {
            Logger.Error(req.error);
        } else {
            Logger.Debug("Uploaded successfully");
        }

        // Now delete the file afterwards
        File.Delete(filePath);
    }
}
