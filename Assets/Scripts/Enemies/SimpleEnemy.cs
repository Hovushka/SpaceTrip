using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public int health = 100;
    public int damage = 40;
    public float deathTime = 1f;

    protected bool gotHit = false;
    protected float hitSide;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.gameObject.GetComponent<SimpleStats>().TakeDamage(damage, gameObject, false);
    }

    public void TakeDamage(int incomingDamage, GameObject hitbox)
    {
        gotHit = true;
        health -= incomingDamage;
        hitSide = Vector3.Angle(hitbox.transform.right * hitbox.GetComponent<Rigidbody2D>().velocity.x, transform.right * transform.localScale.x);
    }
}
