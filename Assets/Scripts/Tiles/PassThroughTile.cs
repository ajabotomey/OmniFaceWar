using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class PassThroughTile : Tile
{

    [Header("Pass Through Tile")]
    [SerializeField][Range(30, 70)] private int minimumOpacity = 30;

    private int maxOpacity = 100;

    private ITilemap tileMap;
    private Vector3Int tilePosition;

    public int MinimumOpacity
    {
        get { return minimumOpacity; }
    }

    public ITilemap TileMap
    {
        get { return tileMap; }
    }

    public Vector3Int TilePosition
    {
        get { return tilePosition; }
        set { tilePosition = value; }
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        this.tileMap = tilemap;
        this.tilePosition = position;

        return base.StartUp(position, tilemap, go);
    }

    public void ModifyOpacity(float opacity = 100)
    {
        // color.a = (opacity / 100);
        Color tileColor = base.color;

        tileColor.a = (opacity / maxOpacity);

        //tileColor.a = (opacity / 100);
        base.color = tileColor;
        Debug.Log("Color of the tile is: " + this.color);
    }

    #region Asset Database
#if UNITY_EDITOR

    [MenuItem("Assets/Custom Tiles/Pass Through Tile")]
    public static void CreatePassThroughTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Pass Through Tile", "PassThroughTile_", "Asset", "Save Pass Through Tile", "Assets");

        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PassThroughTile>(), path);
    }

#endif
    #endregion
}
