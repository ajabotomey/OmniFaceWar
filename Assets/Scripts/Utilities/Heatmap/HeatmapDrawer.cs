using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class HeatmapDrawer : MonoBehaviour
{
    //[SerializeField] private Material material;
    [SerializeField] private HeatmapDataProcessor dataProcessor;
    [SerializeField] private GameObject heatmapSprite;
    [SerializeField] private Texture2D heatTexture;

    private Texture2D heatmapTexture;

    //private const int RED_THRESHOLD = 200;
    //private const int BROWN_THRESHOLD = 100;
    //private const int GREEN_THRESHOLD = 0;

    public void DrawHeatmap()
    {
        //Instantiate(sprite.gameObject, )

        HeatmapData data = dataProcessor.GetHeatmapData();

        for (int i = 0; i < data.positions.Length; i++) {
            GameObject go = Instantiate(heatmapSprite, data.positions[i], Quaternion.identity, transform);
            SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();
            sprite.color = SetColor(data.intensities[i]);
            sprite.size = new Vector2(5, 5);
        }

        // Original code from Zucconi
        //material.SetInt("_Points_Length", data.positions.Length);
        //material.SetVectorArray("_Points", data.positions);

        //Vector4[] properties = new Vector4[data.positions.Length];
        //for (int i = 0; i < data.positions.Length; i++) {
        //    properties[i] = new Vector2(1, data.intensities[i]);
        //}

        //material.SetVectorArray("_Properties", properties);
    }

    private Color SetColor(int intensity)
    {
        Color color;

        if (intensity > 256) {
            color = heatTexture.GetPixel(255, 0);
            color.a = 256;
            return color;
        }

        color = heatTexture.GetPixel(intensity, 0);
        color.a = 256;
        return color;

    }

    public void ClearHeatmap()
    {
        for (var i = transform.childCount; i-- > 0;) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
