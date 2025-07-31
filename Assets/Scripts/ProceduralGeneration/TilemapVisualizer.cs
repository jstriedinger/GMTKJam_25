
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallsTilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase basicWallTile;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        Clear();
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach(var pos in positions)
        {
            PaintSingleTile(pos, tilemap, tile);
        }
    }

    private void PaintSingleTile(Vector2Int pos, Tilemap tilemap, TileBase tile)
    {
        //var tilePosition = tilemap.WorldToCell(new Vector3Int(pos.x, 0, pos.y));
        var tilePosition = new Vector3Int(pos.x, pos.y, 0);
        tilemap.SetTile(tilePosition, tile);
    }

    public void PaintBasicWallTile(Vector2Int pos)
    {
        PaintSingleTile(pos, wallsTilemap, basicWallTile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallsTilemap.ClearAllTiles();
    }
}
