using UnityEngine;
using System.Collections;

public class SimpleShooting : SimpleArmory
{
    public GameObject bullet;
    public float bulletSpeed = 100f;
    public int limit = 3;
    public float ashesTime = 0.1f;

    [HideInInspector] public bool ready = true;
    [HideInInspector] public Coroutine shooting;
    [HideInInspector] public SpriteRenderer ashes;

    private Transform point;
    private float flip = 1f;
    private readonly float EPSILON = 0.00001f;
    private SpriteRenderer self;
    private float timer = -1;
    private SimpleAnimator sa;

    void Awake()
    {
        point = transform.Find("Point");
        point.localPosition = new Vector3(5.5f, 1f, 0f);
        ashes = point.GetComponent<SpriteRenderer>();
        sa = gameObject.GetComponentInParent<SimpleAnimator>();
        self = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ashes.flipX = !sa.flip;

        if (Input.GetButtonDown("Fire1") && ready && GameObject.FindGameObjectsWithTag("Player Hit Box").GetLength(0) < limit)
        {
            shooting = StartCoroutine("Shoot");
        }

        if (Time.time - timer < ashesTime)
            ashes.enabled = true;
        else ashes.enabled = false;
    }

    IEnumerator Shoot()
    {
        ready = false;
        for (int i = 0; i < limit; i++)
        {
            if (sa.isMotionless)
            {
                ready = true;
                ashes.enabled = false;
                break;
            }

            GameObject instance;
            Transform tr;
            instance = Instantiate<GameObject>(bullet);
            tr = instance.GetComponent<Transform>();
            tr.position = new Vector3(point.position.x + 0.5f, point.position.y, point.position.z);
            tr.rotation = point.rotation;
            timer = Time.time;
            flip = (Mathf.Abs(point.localPosition.x - -5.5f) < EPSILON) ? -1f : 1f;
            instance.transform.localScale = new Vector3(flip, instance.transform.localScale.y, instance.transform.localScale.z);
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * flip, 0f);
            yield return new WaitForSeconds(0.1f);
        }
        ready = true;
    }
}
