using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private EnemyBaseAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (enemyAI == null)
        {
            return;
        }

        if (other.tag == "Player")
        {
            enemyAI.SetPlayer(other.gameObject);
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
            enemyAI.SetPlayer(null);
        }
    }
}
