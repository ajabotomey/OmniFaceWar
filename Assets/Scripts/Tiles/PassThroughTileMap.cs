using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PassThroughTileMap : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private GridLayout grid;
    //private Vector3Int tilePosition;

    List<Vector3Int> trackedCells;

    // Start is called before the first frame update
    void Start()
    {
        trackedCells = new List<Vector3Int>();
        //tileMap = GetComponent<Tilemap>();

        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin) {
            TileBase t = tileMap.GetTile(position);

            if (!Equals(t, null)) {
                if (t is PassThroughTile) {
                    PassThroughTile pt = Instantiate(t) as PassThroughTile;
                    pt.StartUp(position, pt.TileMap, pt.gameObject);
                    tileMap.SetTile(position, pt);
                }
            }
        }
    }

    //public void PassThroughEnter(Vector3 contactPoint)
    //{
    //    TileBase tileToPassThrough = tileMap.GetTile(grid.WorldToCell(contactPoint));
    //    if (!Equals(tileToPassThrough, null)) {
    //        if (tileToPassThrough is PassThroughTile) {
    //            ((PassThroughTile)tileToPassThrough).ModifyOpacity(((PassThroughTile)tileToPassThrough).MinimumOpacity);
    //        }
    //    }
    //}

    //public void PassThroughExit(Vector3 contactPoint)
    //{
    //    TileBase tileToPassThrough = tileMap.GetTile(grid.WorldToCell(contactPoint));
    //    if (!Equals(tileToPassThrough, null)) {
    //        if (tileToPassThrough is PassThroughTile) {
    //            ((PassThroughTile)tileToPassThrough).ModifyOpacity();
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollisions(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        HandleCollisions(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        IdentifyIntersections(collider, new BoundsInt(Vector3Int.zero, Vector3Int.zero));
    }

    void HandleCollisions(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<PlayerControl>()) {
            var cellBounds = new BoundsInt(
                grid.WorldToCell(collider.bounds.min), grid.WorldToCell(collider.bounds.size * 32) + new Vector3Int(0, 0, 1));

            IdentifyIntersections(collider, cellBounds);
        }
    }

    void IdentifyIntersections(Collider2D other, BoundsInt cellBounds)
    {
        // Take a copy of the tracked cells
        var exitedCells = trackedCells.ToList();

        // Find intersections within cellBounds
        foreach (var cell in cellBounds.allPositionsWithin) {
            // First check if there's a tile in this cell
            if (tileMap.HasTile(cell)) {
                // Find closest world point to this cell's center within other collider
                var cellWorldCenter = grid.CellToWorld(cell);
                var otherClosestPoint = other.ClosestPoint(cellWorldCenter);
                var otherClosestCell = grid.WorldToCell(otherClosestPoint);

                // Check if intersection point is within this cell
                if (otherClosestCell == cell) {
                    if (!trackedCells.Contains(cell)) {
                        // other collider just entered this cell
                        trackedCells.Add(cell);

                        // Do actions based on other collider entered this cell
                        TileBase tileToPassThrough = tileMap.GetTile(cell);
                        if (!Equals(tileToPassThrough, null)) {
                            if (tileToPassThrough is PassThroughTile) {
                                ((PassThroughTile)tileToPassThrough).ModifyOpacity(((PassThroughTile)tileToPassThrough).MinimumOpacity);

                                tileMap.RefreshTile(((PassThroughTile)tileToPassThrough).TilePosition);
                            }
                        }
                    } else {
                        // other collider remains in this cell, so remove it from the list of exited cells
                        exitedCells.Remove(cell);
                    }
                }
            }
        }

        // Remove cells that are no longer intersected with
        foreach (var cell in exitedCells) {
            trackedCells.Remove(cell);

            // Do actions based on other collider exited this cell
            TileBase tileToPassThrough = tileMap.GetTile(cell);
            if (!Equals(tileToPassThrough, null)) {
                if (tileToPassThrough is PassThroughTile) {
                    ((PassThroughTile)tileToPassThrough).ModifyOpacity();

                    tileMap.RefreshTile(((PassThroughTile)tileToPassThrough).TilePosition);
                }
            }
        }
    }
}
