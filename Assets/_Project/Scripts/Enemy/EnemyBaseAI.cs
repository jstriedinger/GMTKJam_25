using System;
using UnityEngine;

public class EnemyBaseAI : MonoBehaviour
{
    public enum EnemyAIBehavior { Idle, Patrol, Chase, Retreat, Shoot, Attack }

    protected EnemyAIBehavior state = EnemyAIBehavior.Idle;
    [HideInInspector] public GameObject player = null;
    public float speed = 5;
    public float attackDistance = 1;
    public float disengageDistance = 1.3f;
    public float attackCooldown = 0;
    public int damage = 1;
    protected float hitCooldown = 0.1f;

    // Update is called once per frame
    virtual protected void Update()
    {
        UpdateCooldowns();

        switch (state)
        {
            case EnemyAIBehavior.Idle:
                IdleBehavior();
                break;
            case EnemyAIBehavior.Chase:
                ChaseBehavior();
                break;
            case EnemyAIBehavior.Attack:
                AttackBehavior();
                break;
        }
    }

    protected void UpdateCooldowns()
    {
        attackCooldown -= Time.deltaTime;
        hitCooldown -= Time.deltaTime;
    }

    virtual protected void SwitchState(EnemyAIBehavior newState)
    {
        state = newState;
    }

    protected virtual void IdleBehavior()
    {
        if (player != null)
        {
            SwitchState(EnemyAIBehavior.Chase);
        }
    }

    protected virtual void ChaseBehavior()
    {
        if (player == null)
        {
            SwitchState(EnemyAIBehavior.Idle);
            return;
        }

        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        transform.position += new Vector3(playerDirection.x, 0, playerDirection.z) * speed * Time.deltaTime;

        if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {
            SwitchState(EnemyAIBehavior.Attack);
        }
    }

    protected virtual void AttackBehavior()
    {
        if (player == null)
        {
            SwitchState(EnemyAIBehavior.Idle);
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) > disengageDistance)
        {
            SwitchState(EnemyAIBehavior.Chase);
        }
        else
        {
            if (attackCooldown <= 0)
            {
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }

                attackCooldown = 2f;
            }
        }
    }

    public void OnHitByLinedraw()
    {
        // Prevents getting hit by multiple rays
        if (hitCooldown > 0)
        {
            return;
        }
        // Handle hit logic, e.g., play animation, sound, etc.
        Health health = player.GetComponent<Health>();
        if (health == null)
        {
            return;
        }

        hitCooldown = 0.1f;
        health.TakeDamage(1);
        Debug.Log("line hit this enemy");
        if (health.IsDeath())
        {
            Destroy(gameObject);
            Debug.Log("line hit this enemy, die!");
        }
    }
}
