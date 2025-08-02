
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum DungeonType { Tomb, Forest, Temple, Manor }

[System.Serializable]
public class TilePiece
{
    public float weight = 1f;
    public GameObject tile;
}

[System.Serializable]
public class DungeonPieces
{
    public DungeonType dungeonType;
    public List<TilePiece> floorTiles_1x1;
    public List<TilePiece> fullWallTiles;
    public List<TilePiece> northWallTiles_1x1;
    public List<TilePiece> southWallTiles_1x1;
    public List<TilePiece> northWestCornerWallTiles;
    public List<TilePiece> northWestPillarWallTiles;

    private GameObject PickRandomTileWithWeight(List<TilePiece> tiles)
    {
        List<float> cumulativeWeights = new List<float>();
        float total = 0f;
        foreach (TilePiece tile in tiles)
        {
            total += tile.weight;
            cumulativeWeights.Add(total);
        }

        float randomValue = UnityEngine.Random.Range(0f, total);
        for (int i = 0; i < cumulativeWeights.Count; i++)
        {
            if (randomValue <= cumulativeWeights[i])
            {
                return tiles[i].tile;
            }
        }

        return null;
    }

    public GameObject GetFloorTile_1x1()
    {
        return PickRandomTileWithWeight(floorTiles_1x1);
    }

    public GameObject GetNorthWallTile_1x1()
    {
        return PickRandomTileWithWeight(northWallTiles_1x1);
    }

    public GameObject GetSouthWallTile_1x1()
    {
        return PickRandomTileWithWeight(southWallTiles_1x1);
    }

    public GameObject GetNorthWestCornerWallTile()
    {
        return PickRandomTileWithWeight(northWestCornerWallTiles);
    }

    public GameObject GetNorthWestPillarWallTile()
    {
        return PickRandomTileWithWeight(northWestPillarWallTiles);
    }

    public GameObject GetFullWallTiles()
    {
        return PickRandomTileWithWeight(fullWallTiles);
    }
}

public class TilemapVisualizer3D : AbstractTilemapVisualizer
{
    [SerializeField] private Transform levelParent;

    [SerializeField] List<DungeonPieces> dungeonPieces = new List<DungeonPieces>();

    private List<GameObject> spawnedPieces = new List<GameObject>();

    public bool generateSouthWalls = true;

    public override void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        SpawnFloorPieces(floorPositions);
    }

    private void SpawnFloorPieces(IEnumerable<Vector2Int> positions)
    {
        DungeonPieces dungeonPiecesMatch = dungeonPieces.Find(x => x.dungeonType == dungeonType);
        foreach (var pos in positions)
        {
            SpawnSinglePiece(dungeonPiecesMatch.GetFloorTile_1x1(), pos, 0, Quaternion.AngleAxis(UnityEngine.Random.Range(0, 3) * 90, Vector3.up));
        }
    }

    private void SpawnSinglePiece(GameObject piece, Vector2Int pos, float clippingOffset, Quaternion rotation)
    {
        spawnedPieces.Add(Instantiate(piece, new Vector3(pos.x + clippingOffset, clippingOffset, pos.y + clippingOffset), rotation, levelParent));
    }

    // Creates a tiny number to randomly offset the tiles, this prevents clipping during overlap
    private float ClippingFixOffset()
    {
        return UnityEngine.Random.Range(-100f, 100f) / 10000f;
    }

    public override void PaintBasicWallTile(Vector2Int pos, int wallType)
    {
        DungeonPieces dungeonPiecesMatch = dungeonPieces.Find(x => x.dungeonType == dungeonType);
        GameObject piece = null;
        Quaternion orientation = Quaternion.identity;

        if (WallByteTypes3D.wallTop.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWallTile_1x1();
        }
        else if (WallByteTypes3D.wallBottom.Contains(wallType))
        {
            if (generateSouthWalls)
            {
                piece = dungeonPiecesMatch.GetSouthWallTile_1x1();
                orientation *= Quaternion.AngleAxis(180, Vector3.up);
            }
        }
        else if (WallByteTypes3D.wallSideLeft.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWallTile_1x1();
            orientation *= Quaternion.AngleAxis(270, Vector3.up);
        }
        else if (WallByteTypes3D.wallSideRight.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWallTile_1x1();
            orientation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (WallByteTypes3D.wallNorthWest.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWestCornerWallTile();
        }
        else if (WallByteTypes3D.wallNorthEast.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWestCornerWallTile();
            orientation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (WallByteTypes3D.wallSouthEast.Contains(wallType))
        {
            if (generateSouthWalls)
            {
                piece = dungeonPiecesMatch.GetNorthWestCornerWallTile();
                orientation *= Quaternion.AngleAxis(180, Vector3.up);
            }
        }
        else if (WallByteTypes3D.wallSouthWest.Contains(wallType))
        {
            if (generateSouthWalls)
            {
                piece = dungeonPiecesMatch.GetNorthWestCornerWallTile();
                orientation *= Quaternion.AngleAxis(270, Vector3.up);
            }
        }
        else if (WallByteTypes3D.wallFull.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetFullWallTiles();
        }

        if (!piece)
        {
            return;
        }

        SpawnSinglePiece(piece, pos, ClippingFixOffset(), orientation);
    }

    public override void PaintCornerWallTile(Vector2Int pos, int wallType)
    {
        DungeonPieces dungeonPiecesMatch = dungeonPieces.Find(x => x.dungeonType == dungeonType);
        GameObject piece = null;
        Quaternion orientation = Quaternion.identity;

        if (WallByteTypes3D.pillarNorthWest.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWestPillarWallTile();
        }
        else if (WallByteTypes3D.pillarNorthEast.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWestPillarWallTile();
            orientation *= Quaternion.AngleAxis(90, Vector3.up);
        }
        else if (WallByteTypes3D.pillarSouthEast.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWestPillarWallTile();
            orientation *= Quaternion.AngleAxis(180, Vector3.up);
        }
        else if (WallByteTypes3D.pillarSouthWest.Contains(wallType))
        {
            piece = dungeonPiecesMatch.GetNorthWestPillarWallTile();
            orientation *= Quaternion.AngleAxis(270, Vector3.up);
        }

        if (!piece)
        {
            return;
        }

        SpawnSinglePiece(piece, pos, ClippingFixOffset(), orientation);
    }

    public override void Clear()
    {
        var children = levelParent.Cast<Transform>().ToList();
        foreach (Transform t in children)
        {
            Destroy(t.gameObject);
        }

        spawnedPieces.Clear();
    }

    public override void ClearEditor()
    {
        var children = levelParent.Cast<Transform>().ToList();
        foreach (Transform t in children)
        {
            DestroyImmediate(t.gameObject);
        }

        spawnedPieces.Clear();
    }
}
