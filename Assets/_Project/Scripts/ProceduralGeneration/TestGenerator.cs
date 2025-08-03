using System.Collections.Generic;
using UnityEngine;

public class TestGenerator : AbstractDungeonGenerator
{
    protected override void RunProceduralGeneration(bool ranFromEditor)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
        for(int x = -50; x < 50; ++x)
        {
            for (int y = -500; y < 500; ++y)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }
        tilemapVisualizer.PaintFloorTiles(positions);
    }
}
