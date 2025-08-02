using UnityEngine;

[CreateAssetMenu]
public class Prop : ScriptableObject
{
    [Header("Prop data:")]
    public GameObject propData;

    public Vector2Int PropSize = Vector2Int.zero;

    [Space, Header("Placement type:")]
    [Min(0)] public int PlacementQuantityMin = 1;
    [Min(1)] public int PlacementQuantityMax = 1;

    [Space, Header("Group pacement:")]
    public bool PlaceAsGroup = false;
    [Min(1)] public int GroupMinCount = 1;
    [Min(1)] public int GroupMaxCount = 1;

    public bool randomizeRotation = false;
}
