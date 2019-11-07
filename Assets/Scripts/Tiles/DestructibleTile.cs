using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTile : Tile
{
    #region Properties

    [Space(20)]
    [Header("Destructible Tile")]
    [SerializeField] private float life;
    private float startLife;

    [SerializeField] private Sprite brokenSprite;

    private ITilemap tileMap;
    private Vector3Int tilePosition;

    public ITilemap TileMap {
        get { return tileMap; }
    }

    public Vector3Int TilePosition {
        get { return tilePosition; }
        set { tilePosition = value; }
    }

    #endregion


    #region Tile Overriding

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        startLife = life;

        // Store some data
        this.tileMap = tilemap;
        this.tilePosition = position;

        return base.StartUp(position, tilemap, go);
    }

    #endregion

    #region Implementation

    public void ApplyDamage(float damage)
    {
        life -= damage;

        // TODO: Add in broken sprite later once we actually have some to use

        if (life <= 0)
            base.sprite = null;
    }

    #endregion
#if UNITY_EDITOR

    #region Asset Database

    [MenuItem("Assets/Custom Tiles/Destructible Tile")]
    public static void CreateDestructibleTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Destructible Tile", "DestructibleTile_", "Asset", "SaveDestructible Tile", "Assets");

        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<DestructibleTile>(), path);
    }

    #endregion

#endif
}
