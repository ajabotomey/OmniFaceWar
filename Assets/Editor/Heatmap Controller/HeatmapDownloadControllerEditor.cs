using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeatmapDownloadController))]
public class HeatmapDownloadControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HeatmapDownloadController heatmapData = (HeatmapDownloadController)target;

        if (GUILayout.Button("Retrieve Data")) {
            heatmapData.RetrieveData();
        }
    }
}
