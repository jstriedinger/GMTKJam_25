
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer3D : AbstractTilemapVisualizer
{
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject fullWallTile;
    [SerializeField] private GameObject northWallTile;
    [SerializeField] private GameObject northWestCornerWallTile;
    [SerializeField] private GameObject northWestPillarWallTile;

    private List<GameObject> spawnedPieces = new List<GameObject>();

    public bool generateSouthWalls = true;

    public override void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
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
        GameObject piece = null;
        Quaternion orientation = Quaternion.identity;

        if (WallByteTypes3D.wallTop.Contains(wallType))
        {
            piece = northWallTile;
        }
        else if (WallByteTypes3D.wallBottom.Contains(wallType))
        {
            if (generateSouthWalls)
            {
                piece = northWallTile;
                orientation *= Quaternion.AngleAxis(180, Vector3.up);
            }
        }
        else if (WallByteTypes3D.wallSideLeft.Contains(wallType))
        {
            piece = northWallTile;
            orientation *= Quaternion.AngleAxis(270, Vector3.up);
        }
        else if (WallByteTypes3D.wallSideRight.Contains(wallType))
        {
            piece = northWallTile;
            orientation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (WallByteTypes3D.wallNorthWest.Contains(wallType))
        {
            piece = northWestCornerWallTile;
        }
        else if (WallByteTypes3D.wallNorthEast.Contains(wallType))
        {
            piece = northWestCornerWallTile;
            orientation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (WallByteTypes3D.wallSouthEast.Contains(wallType))
        {
            if (generateSouthWalls)
            {
                piece = northWestCornerWallTile;
                orientation *= Quaternion.AngleAxis(180, Vector3.up);
            }
        }
        else if (WallByteTypes3D.wallSouthWest.Contains(wallType))
        {
            if (generateSouthWalls)
            {
                piece = northWestCornerWallTile;
                orientation *= Quaternion.AngleAxis(270, Vector3.up);
            }
        }
        else if (WallByteTypes3D.wallFull.Contains(wallType))
        {
            piece = fullWallTile;
        }

        if (!piece)
        {
            return;
        }

        SpawnSinglePiece(piece, pos, orientation);
    }

    public override void PaintCornerWallTile(Vector2Int pos, int wallType)
    {
        GameObject piece = null;
        Quaternion orientation = Quaternion.identity;

        if (WallByteTypes3D.pillarNorthWest.Contains(wallType))
        {
            piece = northWestPillarWallTile;
        }
        else if (WallByteTypes3D.pillarNorthEast.Contains(wallType))
        {
            piece = northWestPillarWallTile;
            orientation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (WallByteTypes3D.pillarSouthEast.Contains(wallType))
        {
            piece = northWestPillarWallTile;
            orientation *= Quaternion.AngleAxis(180, Vector3.up);
        }
        else if (WallByteTypes3D.pillarSouthWest.Contains(wallType))
        {
            piece = northWestPillarWallTile;
            orientation *= Quaternion.AngleAxis(270, Vector3.up);
        }

        if (!piece)
        {
            return;
        }

        SpawnSinglePiece(piece, pos, orientation);
    }

    public override void Clear()
    {
        var children = levelParent.Cast<Transform>().ToList();
        foreach (Transform t in children)
        {
            DestroyImmediate(t.gameObject);
        }

        spawnedPieces.Clear();
    }
}
