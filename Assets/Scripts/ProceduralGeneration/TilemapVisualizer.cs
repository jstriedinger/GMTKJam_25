
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallsTilemap;
    [SerializeField] private TileBase floorTile;
    [SerializeField]
    private TileBase wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        Clear();
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var pos in positions)
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

    public void PaintBasicWallTile(Vector2Int pos, int wallType)
    {
        TileBase tile = null;
        if (WallByteTypes.wallTop.Contains(wallType))
        {
            tile = wallTop;
        }
        else if (WallByteTypes.wallBottom.Contains(wallType))
        {
            tile = wallBottom;
        }
        else if (WallByteTypes.wallSideLeft.Contains(wallType))
        {
            tile = wallSideLeft;
        }
        else if (WallByteTypes.wallSideRight.Contains(wallType))
        {
            tile = wallSideRight;
        }
        else if (WallByteTypes.wallFull.Contains(wallType))
        {
            tile = wallFull;
        }

        if (!tile)
        {
            return;
        }

        PaintSingleTile(pos, wallsTilemap, tile);
    }

    internal void PaintCornerWallTile(Vector2Int pos, int wallType)
    {
        TileBase tile = null;
        if (WallByteTypes.wallInnerCornerDownLeft.Contains(wallType))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallByteTypes.wallInnerCornerDownRight.Contains(wallType))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerDownRight.Contains(wallType))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallByteTypes.wallDiagonalCornerDownLeft.Contains(wallType))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallByteTypes.wallDiagonalCornerUpRight.Contains(wallType))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallByteTypes.wallDiagonalCornerUpLeft.Contains(wallType))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallByteTypes.wallFullEightDirections.Contains(wallType))
        {
            tile = wallFull;
        }
        else if (WallByteTypes.wallBottomEightDirections.Contains(wallType))
        {
            tile = wallBottom;
        }

        if (!tile)
        {
            return;
        }

        PaintSingleTile(pos, wallsTilemap, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallsTilemap.ClearAllTiles();
    }
}
