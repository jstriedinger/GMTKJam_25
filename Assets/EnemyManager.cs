using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    [SerializeField] private List<EnemyChaseAI> enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CheckLinedrawHit(Vector3[] points)
    {
        foreach (var enemy in enemies)
        {
            
            Vector3 posInPlane = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
            if (IsEnemyHitByPoints(points, posInPlane))
            {
                enemy.OnHitByLinedraw();
            }
        }
    }
    
    private bool IsEnemyHitByPoints(Vector3[] points, Vector3 enemyPos)
    {
        Vector3 enemyPosFlat = new Vector3(enemyPos.x, 0, enemyPos.z);
        foreach (var p in points)
        {
            float dist = Vector3.Distance(new Vector3(p.x, 0, p.z), enemyPosFlat);
            if (dist <= 0.5f)
                return true;
        }
        return false;
    }
}
