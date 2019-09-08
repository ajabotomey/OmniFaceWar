using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeatmapDrawer))]
public class HeatmapDrawerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HeatmapDrawer drawer = (HeatmapDrawer)target;

        if (GUILayout.Button("Draw Heatmap")) {
            drawer.DrawHeatmap();
        }
    }
}
