using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private EnemyChaseAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (enemyAI == null)
        {
            return;
        }

        if (other.tag == "Player")
        {
            enemyAI.player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyAI == null)
        {
            return;
        }

        if (other.tag == "Player")
        {
            enemyAI.player = null;
        }
    }
}
