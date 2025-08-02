using System;
using UnityEngine;

public class EnemyChaseAI : MonoBehaviour
{
    enum EnemyAIBehavior { Idle, Patrol, Chase, Retreat, Shoot, Attack }

    private EnemyAIBehavior state = EnemyAIBehavior.Idle;
    [HideInInspector] public GameObject player = null;
    public float speed = 5;
    public float attackDistance = 1;
    public float disengageDistance = 1.3f;
    public float attackCooldown = 0;
    public int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    private void UpdateCooldowns()
    {
        attackCooldown -= Time.deltaTime;
    }

    private void SwitchState(EnemyAIBehavior newState)
    {
        state = newState;
    }

    private void IdleBehavior()
    {
        if (player != null)
        {
            SwitchState(EnemyAIBehavior.Chase);
        }
    }

    private void ChaseBehavior()
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

    private void AttackBehavior()
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
}
