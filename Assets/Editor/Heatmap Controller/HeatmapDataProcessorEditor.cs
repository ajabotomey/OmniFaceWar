using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeatmapDataProcessor))]
public class HeatmapDataProcessorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HeatmapDataProcessor dataProcessor = (HeatmapDataProcessor)target;

        if (GUILayout.Button("Retrieve Data")) {
            dataProcessor.ProcessData();
        }
    }
}
