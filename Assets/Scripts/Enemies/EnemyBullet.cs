using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float timeAlive = 1.5f;
    public int damage = 40;
    public float angularSpeed = 720f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = angularSpeed;
        Destroy(gameObject, timeAlive);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain") || collision.CompareTag("Obstacle"))
            Destroy(gameObject);

        if (!collision.CompareTag("Enemy Hit Boxes") && !collision.CompareTag("Player Hit Box") && collision.CompareTag("Player"))
        {
            collision.GetComponent<SimpleStats>().TakeDamage(damage, gameObject, false);
            Destroy(gameObject);
        }
    }
}