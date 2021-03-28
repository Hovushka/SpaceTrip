using System.Collections;
using UnityEngine;

public class SimpleGorilla : SimpleEnemy
{
    public float bulletSpeed = 100f;
    public GameObject bullet;
    public float anticipateY = -10;
    public float anticipateStop = 10;
    public float guardLag = 1f;
    public float guardX = 25;
    public float guardY = 10;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D phys;
    private Collider2D shieldUp;
    private Collider2D shieldDown;
    private Transform point;
    private SpriteRenderer body;
    public SpriteRenderer blood;
    private SpriteRenderer arm;
    private GameObject player = null;
    private readonly float EPSILON = 0.01f;
    private float timer = -1f;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        phys = gameObject.GetComponent<Collider2D>();
        shieldDown = transform.Find("Shield Straight").GetComponent<Collider2D>();
        shieldUp = transform.Find("Shield Up").GetComponent<Collider2D>();
        point = transform.Find("Point");
        blood = transform.Find("Blood").GetComponent<SpriteRenderer>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        arm = transform.Find("Arm").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.time - timer < guardLag)
            anim.SetBool("Under Fire", true);
        else anim.SetBool("Under Fire", false);

        if (rb.bodyType != RigidbodyType2D.Static)
            rb.velocity = new Vector2(0f, Physics2D.gravity.y * 3);

        HealthManagement();

        if (player == null)
        {
            GameObject[] list = FindObjectsOfType<GameObject>();
            for (int t = 0; t < list.Length; t++)
                if (list[t].CompareTag("Player"))
                {
                    player = list[t];
                    break;
                }
        }
        else
        {
            float heightDifference = player.transform.root.position.y - transform.root.position.y;
            float widthDifference = player.transform.root.position.x - transform.root.position.x;

            if (widthDifference < 0)
                transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);

            if (heightDifference < anticipateY || heightDifference > anticipateStop)
                anim.SetBool("Anticipate", true);
            else anim.SetBool("Anticipate", false);

            if (heightDifference > guardY && Mathf.Abs(widthDifference) < guardX)
                anim.SetBool("Guard", true);
            else anim.SetBool("Guard", false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("Under Fire", true);
        timer = Time.time + guardLag;
    }

    void HealthManagement()
    {
        if (health <= 0)
        {
            shieldDown.enabled = false;
            shieldUp.enabled = false;
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
            Color armColor = arm.material.color;
            Color bloodColor = blood.material.color;
            Color bodyColor = body.material.color;

            armColor.a = f;
            bloodColor.a = f;
            bodyColor.a = f;

            arm.material.color = armColor;
            blood.material.color = bloodColor;
            body.material.color = bodyColor;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Shoot()
    {
        GameObject instance;
        Transform tr;
        instance = Instantiate<GameObject>(bullet);
        tr = instance.GetComponent<Transform>();
        tr.position = new Vector3(point.position.x + 0.5f, point.position.y, point.position.z);
        tr.rotation = point.rotation;
        float flip = (Mathf.Abs(transform.localScale.x - 1) < EPSILON) ? -1f : 1f;
        instance.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * flip, 0f);
    }
}
