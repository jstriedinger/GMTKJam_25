using System.Collections.Generic;
using UnityEngine;

public class CorridorDungeonGenerator : SimpleWalkDungeonGenerator
{
    protected void WidenCorridors(HashSet<Vector2Int> floorPositions, List<List<Vector2Int>> corridors, int widenCorridorSize)
    {
        for (int i = 0; i < corridors.Count; ++i)
        {
            corridors[i] = IncreaseCorridorBrushBy(corridors[i], widenCorridorSize);
            floorPositions.UnionWith(corridors[i]);
        }
    }

    private List<Vector2Int> IncreaseCorridorBrushBy(List<Vector2Int> corridor, int widenCorridorSize)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        int start = Mathf.FloorToInt(widenCorridorSize / 2.0f) * -1;
        int end = Mathf.CeilToInt(widenCorridorSize / 2.0f);
        for (int i = 0; i < corridor.Count; i++)
        {
            for (int x = start; x < end; x++)
            {
                for (int y = start; y < end; y++)
                {
                    newCorridor.Add(corridor[i] + new Vector2Int(x, y));
                }
            }
        }

        return newCorridor;
    }
}
