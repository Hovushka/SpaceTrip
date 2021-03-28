using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleGoat : SimpleEnemy
{
    public float movementSpeed = 30f;
    public float rotationSpeed = 100f;
    public float injureTime = 0.15f;

    private Animator anim;
    private Collider2D phys;
    private Rigidbody2D rb;
    private Rigidbody2D player = null;
    private SpriteRenderer mouth;
    private SpriteRenderer blood;
    private SpriteRenderer head;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        phys = gameObject.GetComponent<Collider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        mouth = gameObject.GetComponent<SpriteRenderer>();
        blood = transform.Find("Blood").GetComponent<SpriteRenderer>();
        head = transform.Find("Head").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (rb.bodyType != RigidbodyType2D.Static)
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            }
            else if (health > 0)
            {
                Vector2 xy = player.position - rb.position;
                xy.Normalize();

                float rotation = Vector3.Cross(xy, transform.up).z;

                rb.angularVelocity = -1 * rotation * rotationSpeed;
                rb.velocity = transform.up * movementSpeed;

                if (transform.eulerAngles.z > 180)
                    transform.localScale = new Vector3(-1, 1, 1);
                else transform.localScale = new Vector3(1, 1, 1);
            }
        }

        HealthManagement();
    }

    void HealthManagement()
    {
        if (health <= 0)
        {
            blood.enabled = true;
            StartCoroutine("FadeOut");
            Destroy(gameObject, deathTime);
            anim.SetBool("Is Dead", true);
            phys.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0f; f -= 0.04f)
        {
            Color legsColor = mouth.material.color;
            Color bloodColor = blood.material.color;
            Color bodyColor = head.material.color;

            legsColor.a = f;
            bloodColor.a = f;
            bodyColor.a = f;

            mouth.material.color = legsColor;
            blood.material.color = bloodColor;
            head.material.color = bodyColor;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
