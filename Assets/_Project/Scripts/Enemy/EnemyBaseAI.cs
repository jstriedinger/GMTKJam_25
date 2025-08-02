using System;
using UnityEngine;

public class EnemyBaseAI : MonoBehaviour
{
    public enum EnemyAIBehavior { Idle, Patrol, Chase, Retreat, Shoot, Attack }

    protected EnemyAIBehavior state = EnemyAIBehavior.Idle;
    protected GameObject player = null;

    [SerializeField] protected float speed = 5;
    [SerializeField] protected float attackDistance = 1;
    [SerializeField] protected float disengageDistance = 1.3f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float shootRange = 3;
    [SerializeField] protected float shootCooldown = 2f;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float projectileForce = 5f;

    protected float attackCooldown = 0;
    protected float hitCooldown = 0;
    protected float currentShootCooldown = 0;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;

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
            case EnemyAIBehavior.Retreat:
                RetreatBehavior();
                break;
            case EnemyAIBehavior.Shoot:
                ShootBehavior();
                break;
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    protected void UpdateCooldowns()
    {
        attackCooldown -= Time.deltaTime;
        hitCooldown -= Time.deltaTime;
        currentShootCooldown -= Time.deltaTime;
    }

    private void Movement()
    {
        float rateOfChange = 5f;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, rateOfChange * Time.deltaTime);

        if (currentVelocity.magnitude != 0)
        {
            transform.position += currentVelocity * Time.deltaTime;
        }

        targetVelocity = Vector3.Lerp(targetVelocity, Vector3.zero, rateOfChange * Time.deltaTime);
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
        targetVelocity = new Vector3(playerDirection.x, 0, playerDirection.z) * speed;

        if (Vector3.Distance(player.transform.position, transform.position) <= attackDistance)
        {
            SwitchState(EnemyAIBehavior.Attack);
        }
    }

    protected virtual void RetreatBehavior()
    {
        if (player == null)
        {
            SwitchState(EnemyAIBehavior.Idle);
            return;
        }

        Vector3 playerDirection = (transform.position - player.transform.position).normalized;
        targetVelocity = new Vector3(playerDirection.x, 0, playerDirection.z) * speed;
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

    protected virtual void ShootBehavior()
    {
        if (player == null)
        {
            SwitchState(EnemyAIBehavior.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 playerDirection = Vector3.zero;
        if (distance > shootRange)
        {
            playerDirection = (player.transform.position - transform.position).normalized;
        }
        else if (distance < shootRange - 0.5f)
        {
            playerDirection = (transform.position - player.transform.position).normalized;
        }
        targetVelocity = new Vector3(playerDirection.x, 0, playerDirection.z) * speed;

        if (currentShootCooldown <= 0)
        {
            currentShootCooldown = shootCooldown;
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        if (player == null)
        {
            return;
        }

        Vector3 playerDirection = (player.transform.position + new Vector3(0, 0.5f, 0) - transform.position).normalized;
        GameObject newProjectile = Instantiate(projectile, transform.position + playerDirection * 0.5f, Quaternion.identity);
        newProjectile.GetComponent<EnemyProjectile>().FlyToEnemy(playerDirection, projectileForce);
    }

    public void OnHitByLinedraw()
    {
        // Prevents getting hit by multiple rays
        if (hitCooldown > 0)
        {
            return;
        }
        // Handle hit logic, e.g., play animation, sound, etc.
        Health health = GetComponent<Health>();
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

    public virtual void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
