using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    public static event Action<GameObject> takenDamageEvent;
    [SerializeField] public int totalHealth;

    public void TakeDamage(int damage)
    {
        totalHealth = Mathf.Max(0, totalHealth - damage);
        takenDamageEvent?.Invoke(gameObject);
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
