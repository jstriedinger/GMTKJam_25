using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public int totalHealth = 3;

    [Header("Heart UI Elements")]
    [SerializeField] private Image first;
    [SerializeField] private Image second;
    [SerializeField] private Image third;

    [Header("Heart Sprites")]
    [SerializeField] private Sprite fullheart;
    [SerializeField] private Sprite noheart;

    public void RegainHealth()
    {
        third.sprite = fullheart;
        second.sprite = fullheart;
        first.sprite = fullheart;
    }

    public void TakeDamage(int damage)
    {
        totalHealth = Mathf.Max(0, totalHealth - damage);

        // Reset all to full first
        first.sprite = fullheart;
        second.sprite = fullheart;
        third.sprite = fullheart;

        // Now turn off hearts based on missing health
        if (totalHealth <= 2)
            third.sprite = noheart;
        if (totalHealth <= 1)
            second.sprite = noheart;
        if (totalHealth <= 0)
        {
            first.sprite = noheart;
            // DEATH ANIMATION
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.TeleportPlayerToHub();
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
