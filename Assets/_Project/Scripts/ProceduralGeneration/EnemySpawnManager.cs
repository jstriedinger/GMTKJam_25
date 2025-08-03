using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyType { BasicShadow, FasterShadow, HelplessScaredShadow, DefensiveRangedShadow }

[System.Serializable]
public class EnemySpawnRules
{
    public EnemyType enemyType = EnemyType.BasicShadow;
    public int minEnemiesInRoom = 0;
    public int maxEnemiesInRoom = 0;
    public int damage = 1;
    public int health = 1;
}

[System.Serializable]
public class EnemyRoomSpawnRules
{
    public List<EnemySpawnRules> spawnRules;
}

[System.Serializable]
public class DungeonEnemySettings
{
    public DungeonType dungeonType;
    public List<EnemyRoomSpawnRules> roomSpawnRules;
}

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject basicShadow;
    [SerializeField] private GameObject fasterShadow;
    [SerializeField] private GameObject helplessScaredShadow;
    [SerializeField] private GameObject defensiveRangedShadow;
    [SerializeField] private Dungeon dungeonData;
    [SerializeField] private Transform levelParent;
    [SerializeField] private List<DungeonEnemySettings> allDungeonEnemySettings;

    public void ProcessRooms(DungeonType dungeonType)
    {
        DungeonEnemySettings dungeonSettings = allDungeonEnemySettings.Find(x => x.dungeonType == dungeonType);
        if (dungeonSettings == null)
        {
            // No enemies for this dungeon type
            return;
        }

        foreach (Room room in dungeonData.rooms)
        {
            PlaceEnemies(room, dungeonSettings.roomSpawnRules, room.innerTiles);
        }
    }

    private void PlaceEnemies(Room room, List<EnemyRoomSpawnRules> roomSpawnRules, HashSet<Vector2Int> availableTiles)
    {
        if (roomSpawnRules.Count == 0)
        {
            return;
        }

        HashSet<Vector2Int> tempPositons = new HashSet<Vector2Int>(availableTiles);
        EnemyRoomSpawnRules selectedRoomSpawnRules = roomSpawnRules[Random.Range(0, roomSpawnRules.Count)];

        foreach (EnemySpawnRules spawnRules in selectedRoomSpawnRules.spawnRules)
        {
            //We want to place only certain quantity of enemies
            int quantity = UnityEngine.Random.Range(spawnRules.minEnemiesInRoom, spawnRules.maxEnemiesInRoom + 1);
            for (int i = 0; i < quantity; i++)
            {
                //remove taken positions
                tempPositons.ExceptWith(room.enemyPositions);
                //shuffle the positions
                List<Vector2Int> availablePositions = tempPositons.OrderBy(x => System.Guid.NewGuid()).ToList();
                //If placement has failed there is no point in trying to place the same enemy again
                if (!TryPlacingEnemy(room, spawnRules.enemyType, spawnRules.health, spawnRules.damage, availablePositions))
                    break;
            }

        }
    }

    private bool TryPlacingEnemy(Room room, EnemyType enemyType, int health, int damage, List<Vector2Int> availablePositions) {

        for (int i = 0; i < availablePositions.Count; i++)
        {
            Vector2Int position = availablePositions[i];
            if (room.enemyPositions.Contains(position))
                continue;

            SpawnEnemy(position, enemyType, health, damage);

            room.enemyPositions.Add(position);
            return true;
        }

        return false;
    }

    private void SpawnEnemy(Vector2Int placementPosition, EnemyType enemyType, int health, int damage)
    {
        GameObject enemyToSpawn = GetEnemy(enemyType);
        if (enemyToSpawn == null)
        {
            return;
        }

        float offsetAmount = 0.25f;
        Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f) * offsetAmount, 0, Random.Range(-1f, 1f) * offsetAmount);
        GameObject spawnedEnemy = Instantiate(enemyToSpawn, new Vector3(placementPosition.x, 0.6f, placementPosition.y) + randomOffset, Quaternion.identity, levelParent);
        
        EnemyBaseAI enemyAIComponent = spawnedEnemy.GetComponent<EnemyBaseAI>();
        if(enemyAIComponent != null)
        {
            enemyAIComponent.damage = damage;
        }

        Health healthComponent = spawnedEnemy.GetComponent<Health>();
        if (healthComponent != null)
        {
            healthComponent.totalHealth = health;
        }
    }

    private GameObject GetEnemy(EnemyType enemyType)
    {
        switch (enemyType) {
            case EnemyType.BasicShadow:
                return basicShadow;
            case EnemyType.FasterShadow:
                return fasterShadow;
            case EnemyType.HelplessScaredShadow:
                return helplessScaredShadow;
            case EnemyType.DefensiveRangedShadow:
                return defensiveRangedShadow;
        }

        return null;
    }
}
