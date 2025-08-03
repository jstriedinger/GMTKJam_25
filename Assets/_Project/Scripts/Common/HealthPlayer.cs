using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{

    public static event Action<GameObject> takenDamageEvent;
    [SerializeField] public int totalHealth = 3;

    [Header("Heart UI Elements")]
    [SerializeField] private List<Image> hearts;

    [Header("Heart Sprites")]
    [SerializeField] private Sprite fullheart;
    [SerializeField] private Sprite noheart;

    public void RegainHealth()
    {
        foreach (var heart in hearts)
        {
            heart.sprite = fullheart;
        }
    }

    public void TakeDamage(int damage)
    {
        totalHealth = Mathf.Max(0, totalHealth - damage);

        // Reset all to full first
        foreach (var heart in hearts)
        {
            heart.sprite = fullheart;
            if (totalHealth <= 2)
                heart.sprite = noheart;
            if (totalHealth <= 1)
                heart.sprite = noheart;
            if (totalHealth <= 0)
            {
                heart.sprite = noheart;
                Die();
            }
        }

        takenDamageEvent?.Invoke(gameObject);
    }
    private void Die()
    {
        if (true)
        {
            GameManager.Instance.TeleportPlayerToHub();
        }
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
