using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int totalHealth;

    public void TakeDamage(int damage)
    {
        totalHealth = Mathf.Max(0, totalHealth - damage);
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
