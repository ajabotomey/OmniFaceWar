using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructibleTileMap : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private GridLayout grid;
    private Vector3Int tilePosition;

    public void Start()
    {
        tileMap = GetComponent<Tilemap>();

        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin) {
            TileBase t = tileMap.GetTile(position);

            if (!Equals(t, null)) {
                if (t is DestructibleTile) {
                    DestructibleTile dt = Instantiate(t) as DestructibleTile;
                    dt.StartUp(position, dt.TileMap, dt.gameObject);
                    tileMap.SetTile(position, dt);
                }
            }
        }
    }

    public void Damage(Vector3 contactPoint, int damage)
    {
        TileBase tileToDamage = tileMap.GetTile(grid.WorldToCell(contactPoint));
        if (!Equals(tileToDamage, null)) {
            if (tileToDamage is DestructibleTile) {
                ((DestructibleTile)tileToDamage).ApplyDamage(damage);

                tileMap.RefreshTile(((DestructibleTile)tileToDamage).TilePosition);
            }
        }
    }
}
