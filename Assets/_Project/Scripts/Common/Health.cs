using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int totalHealth;

    public void TakeDamage(int damage)
    {
        totalHealth = Mathf.Max(0, totalHealth - damage);
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
