using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class HeatmapData
{
    public Vector4[] positions;
    public int[] intensities;

    public void Initialize(int size)
    {
        positions = new Vector4[size];
        intensities = new int[size];
    }
}

[ExecuteInEditMode]
public class HeatmapDataProcessor : MonoBehaviour
{
    [SerializeField] private HeatmapDownloadController download;

    private char[] separators = { '(', ')', ',' };
    private char newlineSeparator = '\n';

    private Dictionary<Vector2, int> positionList; // Position, intensity
    private HeatmapData heatmapData;

    private int highestCount = 0;

    //public void ProcessData()
    //{
    //    positionList = new Dictionary<Vector2, int>();
    //    string data = download.GetData();

    //    string[] vectorArray = data.Split(newlineSeparator);

    //    for (int i = 0; i < vectorArray.Length - 1; i++) {
    //        Vector2 pos = GetVector2FromLine(vectorArray[i]);
            
    //        if (positionList.ContainsKey(pos)) {
    //            positionList[pos] += 1;
    //        } else {
    //            positionList.Add(pos, 1);
    //        }
    //    }
    //}

    public void ProcessData()
    {
        if (positionList == null)
            positionList = new Dictionary<Vector2, int>();
        else
            positionList.Clear();

        string data = download.GetData();

        JSONReader.ReadPositionList(ref positionList, ref data, ref highestCount);
    }

    private Vector2 GetVector2FromLine(string positionString)
    {
        // Should only have two values after split
        string[] textChunks = positionString.Split(separators);

        Vector2 pos = new Vector2();

        pos.x = float.Parse(textChunks[1]);
        pos.y = float.Parse(textChunks[2]);

        return pos;
    }

    // Converts position dictionary into a struct I can use in the Heatmap Drawer class
    public HeatmapData GetHeatmapData()
    {
        if (positionList.Count == 0) {
            Logger.Error("Need to retrieve data from the server first");
            return new HeatmapData();
        }

        heatmapData = new HeatmapData();
        heatmapData.Initialize(positionList.Count);

        int i = 0;
        foreach (KeyValuePair<Vector2, int> entry in positionList) {
            heatmapData.positions[i] = entry.Key;
            heatmapData.intensities[i] = entry.Value * GetIntensityScale(); // TODO: Process separately later to accurately display color
            i++;
        }

        return heatmapData;
    }

    static public bool JsonDataContainsKey(JsonData data, string key)
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

    private int GetIntensityScale()
    {
        int highestIntensity = 255;
        while (highestCount > highestIntensity)
            highestIntensity *= 2;

        Debug.Log(highestIntensity);

        return 255 / highestIntensity;
    }
}
