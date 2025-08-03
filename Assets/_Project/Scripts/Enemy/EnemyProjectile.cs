using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float despawnTime = 5f;

    private void Update()
    {
        despawnTime -= Time.deltaTime;
        if (despawnTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FlyToEnemy(Vector3 playerDirection, float force)
    {
        rigidBody.AddForce(playerDirection * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HealthPlayer health = other.GetComponent<HealthPlayer>();
            if (health != null)
            {
                health.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
