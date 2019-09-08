using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HeatmapDrawer : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private HeatmapDataProcessor dataProcessor;

    public void DrawHeatmap()
    {
        HeatmapData data = dataProcessor.GetHeatmapData();

        material.SetInt("_Points_Length", data.positions.Length);
        material.SetVectorArray("_Points", data.positions);

        Vector4[] properties = new Vector4[data.positions.Length];
        for (int i = 0; i < data.positions.Length; i++) {
            properties[i] = new Vector2(1, data.intensities[i]);
        }

        material.SetVectorArray("_Properties", properties);
    }
}
