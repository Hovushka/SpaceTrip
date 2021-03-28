using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SimpleJump), typeof(Rigidbody2D))]
public class SimpleStats : MonoBehaviour
{
    public int health = 100;
    public float deathTime = 1f;
    public float invincibility = 0.5f;
    public float kickBackY = 1000f;
    public float kickBackX = 1000f;
    public float motionLess = 5f;

    [HideInInspector] public bool isInvincible = false;
    [HideInInspector] public float timer = -5f;

    private Rigidbody2D rb;
    private SimpleJump sj;
    private SimpleMovement sm;
    private SimpleArmory ssh;
    private bool animateBlood = true;
    private Canvas can = null;
    private float gravity;

    void Awake()
    {
        sj = gameObject.GetComponent<SimpleJump>();
        sm = gameObject.GetComponent<SimpleMovement>();
        ssh = gameObject.GetComponentInChildren<SimpleArmory>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    public void TakeDamage(int damage, GameObject enemy, bool overRide)
    {
        if (!animateBlood)
            return;

        if (overRide || Time.time - timer > invincibility) {
            health -= damage;
            timer = Time.time;

            if (health <= 0)
            {
                Destroy(gameObject, deathTime);
                animateBlood = false;
            }
            else
            {

                float angle = (gameObject.transform.root.position.x - enemy.transform.root.position.x > 0) ? 1 : -1;
                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(kickBackX * angle, kickBackY));
                sj.ascending = false;
                sm.enabled = false;
                sj.enabled = false;
            }
        }
    }

    void Update()
    {
        isInvincible = Time.time - timer <= invincibility;

        if (can == null)
            can = FindObjectOfType<Canvas>();
        else can.transform.Find("Health").GetComponent<Text>().text = health.ToString();
    }
}
