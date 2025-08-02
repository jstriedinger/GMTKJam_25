using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int totalHealth;
    [SerializeField] public SpriteRenderer first;
    [SerializeField] public SpriteRenderer second;
    [SerializeField] public SpriteRenderer third;
    [SerializeField] public SpriteRenderer fullheart;
    [SerializeField] public SpriteRenderer noheart;

    public void TakeDamage(int damage)
    {
        totalHealth = Mathf.Max(0, totalHealth - damage);
        if (totalHealth == 2)
        {

        }
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
