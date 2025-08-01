using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public BoundsInt bounds;
    public Vector2Int center;
    public HashSet<Vector2Int> floorPositions;
    public HashSet<Vector2Int> innerTiles;
    public HashSet<Vector2Int> propPositions = new HashSet<Vector2Int>();
    public List<GameObject> propObjectReferences = new List<GameObject>();

    public Room(BoundsInt bounds, HashSet<Vector2Int> floorPositions)
    {
        this.bounds = bounds;
        center = Vector2Int.RoundToInt(bounds.center);
        this.floorPositions = floorPositions;

        CalculateInnerTiles();
    }

    private void CalculateInnerTiles()
    {
        innerTiles = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            bool isInnerTile = true;
            foreach (var direction in ProceduralGeneration.Direction2D.CardinalDirectionsList)
            {
                if (!floorPositions.Contains(position + direction))
                {
                    isInnerTile = false;
                    break;
                }
            }

            if (isInnerTile)
            {
                innerTiles.Add(position);
            }
        }
    }
}