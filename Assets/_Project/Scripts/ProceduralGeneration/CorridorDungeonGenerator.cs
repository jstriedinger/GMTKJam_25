using System.Collections.Generic;
using UnityEngine;

public class CorridorDungeonGenerator :SimpleWalkDungeonGenerator
{
    [SerializeField] protected bool widenCorridors = true;

    protected void WidenCorridors(HashSet<Vector2Int> floorPositions, List<List<Vector2Int>> corridors)
    {
        for (int i = 0; i < corridors.Count; ++i)
        {
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }
    }

    private List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 0; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i] + new Vector2Int(x, y));
                }
            }
        }

        return newCorridor;
    }
}
