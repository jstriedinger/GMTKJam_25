using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected DungeonGenerationData generationParams;
    [SerializeField] protected AbstractTilemapVisualizer tilemapVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tilemapVisualizer.dungeonType = generationParams.dungeonType;
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    public void ClearDungeon()
    {
        tilemapVisualizer.Clear();
    }

    public void ClearDungeonEditor()
    {
        tilemapVisualizer.ClearEditor();
    }

    protected abstract void RunProceduralGeneration();
}
