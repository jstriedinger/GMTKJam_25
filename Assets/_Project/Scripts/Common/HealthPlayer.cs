using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{

    [SerializeField] private Animator charAnimator;
    public static event Action playerDamageEvent;
    public static event Action playerDiedEvent;
    [SerializeField] public int totalHealth;
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


        // Update heart sprites based on current health
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < totalHealth)
                hearts[i].sprite = fullheart;
            else
                hearts[i].sprite = noheart;
        }


        playerDamageEvent?.Invoke();

        if (totalHealth > 0)
            StartCoroutine(_playerMovement.OnTakingDamage());
        else
        {
            StartCoroutine(Die());
        }


    }

    IEnumerator Die()
    {
        playerDiedEvent?.Invoke();
        _playerMovement.ToggleCanMove(false);
        charAnimator.SetBool("Dead", true);
        _canTakeDamage = false;
        charAnimator.ResetTrigger("Hurt");
        yield return new WaitForSeconds(3);
        GameManager.Instance?.TeleportPlayerToHub();
        charAnimator.SetBool("Dead", false);
        charAnimator.SetTrigger("Spawn");
        RegainHealth();
        _playerMovement.ToggleCanMove(true);
    }

    public bool IsDeath()
    {
        return totalHealth <= 0;
    }
}
