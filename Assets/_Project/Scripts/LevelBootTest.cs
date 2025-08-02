using UnityEngine;

public class LevelBootTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private AbstractDungeonGenerator generator;

    void Start()
    {
        generator.GenerateDungeon();
    }
}
