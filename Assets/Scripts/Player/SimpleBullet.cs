using UnityEngine;
using System.Collections;

public class SimpleBullet : MonoBehaviour
{
    public Animator anim;
    public float timeAlive = 1.5f;
    public int damage = 40;
    public float kickback = 1500f;
    public float angularSpeed = 720f;
    public float secondDeath = 0.4f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D phys;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        phys = gameObject.GetComponent<Collider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        Invoke("DestroyMe", timeAlive);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain") || collision.CompareTag("Obstacle"))
        {
            CancelInvoke("DestroyMe");
            phys.enabled = false;
            float angle = (rb.velocity.x > 0) ? -1 : 1f;
            rb.velocity = new Vector2(0f, 0f);
            rb.AddForce(new Vector2(angle * kickback * Random.Range(0.5f, 1f), kickback * Random.Range(-1f, 1f)));
            rb.freezeRotation = false;
            rb.angularVelocity = angularSpeed;
            transform.localScale = new Vector3(0.5f, 1, 1);
            StartCoroutine("FadeOut");
            Destroy(gameObject, secondDeath);
        }

        if (!collision.CompareTag("Enemy Hit Boxes") && !collision.CompareTag("Player Hit Box") && collision.CompareTag("Enemy"))
        {
            CancelInvoke("DestroyMe");
            collision.GetComponent<SimpleEnemy>().TakeDamage(damage, gameObject);
            phys.enabled = false;
            rb.velocity = new Vector2(0f, 0f);
            rb.bodyType = RigidbodyType2D.Static;
            anim.SetBool("Penetrate", true);
            Destroy(gameObject, 0.2f);
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0f; f -= 0.045f)
        {
            Color mew = sr.material.color;
            mew.a = f;
            sr.material.color = mew;

            yield return new WaitForSeconds(0.01f);
        }
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}