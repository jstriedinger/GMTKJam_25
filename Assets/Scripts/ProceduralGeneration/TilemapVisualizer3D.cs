
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer3D : AbstractTilemapVisualizer
{
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject basicWallTile;
    [SerializeField] private GameObject northWallTile;
    [SerializeField]
    private TileBase wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    private List<GameObject> spawnedPieces = new List<GameObject>();

    public bool generateSouthWalls = true;

    public override void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        Clear();
        SpawnFloorPieces(floorPositions);
    }

    private void SpawnFloorPieces(IEnumerable<Vector2Int> positions)
    {
        foreach (var pos in positions)
        {
            SpawnSinglePiece(floorTile, pos, Quaternion.identity);
        }
    }

    private void SpawnSinglePiece(GameObject piece, Vector2Int pos, Quaternion rotation)
    {
        spawnedPieces.Add(Instantiate(piece, new Vector3Int(pos.x, 0, pos.y), rotation, levelParent));
    }

    public override void PaintBasicWallTile(Vector2Int pos, int wallType)
    {
        //SpawnSinglePiece(basicWallTile, pos, Quaternion.identity);
        //return;

        GameObject piece = basicWallTile;
        Quaternion orientation = Quaternion.identity;
        TileBase tile = null;
        if (WallByteTypes3D.wallTop.Contains(wallType))
        {
            piece = northWallTile;
        }
        else if (WallByteTypes3D.wallBottom.Contains(wallType))
        {
            if (!generateSouthWalls)
            {
                piece = null;
            }
                //piece = northWallTile;
                //piece = basicWallTile;
                //orientation *= Quaternion.AngleAxis(180, Vector3.up);
        }
        else if (WallByteTypes.wallSideLeft.Contains(wallType))
        {
            //piece = northWallTile;
            //orientation *= Quaternion.AngleAxis(270, Vector3.up);
        }
        else if (WallByteTypes.wallSideRight.Contains(wallType))
        {
            //piece = northWallTile;
            //orientation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (WallByteTypes.wallFull.Contains(wallType))
        {
            tile = wallFull;
        }

        if (!piece)
        {
            return;
        }

        SpawnSinglePiece(piece, pos, orientation);
    }

    public override void PaintCornerWallTile(Vector2Int pos, int wallType)
    {
        //SpawnSinglePiece(basicWallTile, pos, Quaternion.identity);

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
    }

    public override void Clear()
    {
        foreach(var piece in spawnedPieces)
        {
            DestroyImmediate(piece);
        }

        spawnedPieces.Clear();
    }
}
