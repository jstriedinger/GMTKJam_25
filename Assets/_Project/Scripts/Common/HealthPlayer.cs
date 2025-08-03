using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{

    [SerializeField] private Animator charAnimator;
    public static event Action<GameObject> takenDamageEvent;
    [SerializeField] public int totalHealth = 3;
    int _initialHealth;
    private bool _canTakeDamage = true;
    

    [Header("Heart UI Elements")]
    [SerializeField] private List<Image> hearts;

    [Header("Heart Sprites")]
    [SerializeField] private Sprite fullheart;
    [SerializeField] private Sprite noheart;
    
    PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _initialHealth = totalHealth;
    }

    public void RegainHealth()
    {
        totalHealth = _initialHealth;
        foreach (var heart in hearts)
        {
            heart.sprite = fullheart;
        }

        _canTakeDamage = true;
    }

    public void TakeDamage(int damage)
    {
        if (!_canTakeDamage)
            return;
        
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
            }
        }

        takenDamageEvent?.Invoke(gameObject);
        if (totalHealth > 0)
            StartCoroutine(_playerMovement.OnTakingDamage());
        else
        {
            StartCoroutine(Die());
        }
        

    }
    IEnumerator Die()
    {
        _playerMovement.ToggleCanMove(false);
        charAnimator.SetTrigger("Dead");
        _canTakeDamage = false;
        yield return new WaitForSeconds(3);
        GameManager.Instance?.TeleportPlayerToHub();
        charAnimator.SetTrigger("Spawn");
        RegainHealth();
        _playerMovement.ToggleCanMove(true);
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
