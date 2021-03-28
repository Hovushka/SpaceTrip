using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(SimpleGround), typeof(SimpleWall))]
public class SimpleBull : SimpleEnemy
{
    public float fallSpeed = 3f;
    public float speed = 50f;
    public float angle = -1;

    private SpriteRenderer legs;
    private SpriteRenderer blood;
    private SpriteRenderer body;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D phys;
    private SimpleGround sg;
    private SimpleWall sw;
    private Collider2D groundCollider;
    private Collider2D wallCollider;
    private readonly float EPSILON = 0.01f;
    private bool isGrounded;
    
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        phys = gameObject.GetComponent<Collider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        legs = gameObject.GetComponent<SpriteRenderer>();
        blood = transform.Find("Blood").GetComponent<SpriteRenderer>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        sg = transform.Find("Ground").GetComponent<SimpleGround>();
        sw = transform.Find("Wall").GetComponent<SimpleWall>();
        groundCollider = transform.Find("Ground").GetComponent<Collider2D>();
        wallCollider = transform.Find("Wall").GetComponent<Collider2D>();
    }

    void Update()
    {
        if (rb.bodyType != RigidbodyType2D.Static)
        {
            if ((!sg.isGrounded || sw.isWalled) && isGrounded)
            {
                angle *= -1;
                transform.localScale = (Mathf.Abs(angle - -1) < EPSILON) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            }
            Vector2 movement = new Vector2(speed * angle, Physics2D.gravity.y * fallSpeed);
            if (isGrounded) movement.y = 0f;
            rb.velocity = movement;
        }

        HealthManagement();
    }

    void HealthManagement()
    {
        if (health <= 0)
        {
            groundCollider.enabled = false;
            wallCollider.enabled = false;
            blood.enabled = true;
            body.enabled = true;
            StartCoroutine("FadeOut");
            Destroy(gameObject, deathTime);
            anim.SetBool("Is Dead", true);
            phys.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;

            if (Mathf.Abs(hitSide - 180) < EPSILON)
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                hitSide = 0;
            }
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0f; f -= 0.04f)
        {
            Color legsColor = legs.material.color;
            Color bloodColor = blood.material.color;
            Color bodyColor = body.material.color;

            legsColor.a = f;
            bloodColor.a = f;
            bodyColor.a = f;

            legs.material.color = legsColor;
            blood.material.color = bloodColor;
            body.material.color = bodyColor;

            yield return new WaitForSeconds(0.01f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
            isGrounded = false;
    }
}
