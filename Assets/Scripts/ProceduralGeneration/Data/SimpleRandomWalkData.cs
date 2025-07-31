using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_", menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkData :ScriptableObject
{
    public int iterations = 10;
    public int walkLenth = 10;
    public bool startRandomlyEachIteration = true;
}
