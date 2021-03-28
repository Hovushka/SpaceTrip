using UnityEngine;

[RequireComponent(typeof(SimpleJump), typeof(SimpleMovement))]
[RequireComponent(typeof(SimpleStats), typeof(Animator), typeof(Rigidbody2D))]
public class SimpleAnimator : MonoBehaviour
{
    public float flickerInterval = 0.01f;
    public bool flip;

    [HideInInspector] public bool isMotionless = false;

    private Animator anim;
    private Rigidbody2D rb;
    private SimpleJump sj;
    private SimpleMovement sm;
    private SimpleArmory ssh;
    private SimpleStats st;
    private SpriteRenderer body;
    private SpriteRenderer blood;
    private SpriteRenderer shoulder;
    private SpriteRenderer back;
    private SpriteRenderer renderGun;
    private Transform gun;
    private Transform point;

    private readonly float EPSILON = 0.0001f;
    private float timer;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sj = gameObject.GetComponent<SimpleJump>();
        sm = gameObject.GetComponent<SimpleMovement>();
        ssh = gameObject.GetComponentInChildren<SimpleArmory>();
        st = gameObject.GetComponent<SimpleStats>();
        body = gameObject.GetComponent<SpriteRenderer>();
        blood = transform.Find("Blood").GetComponent<SpriteRenderer>();
        shoulder = transform.Find("Shoulder Pad").GetComponent<SpriteRenderer>();
        back = transform.Find("Shoulder Back").GetComponent<SpriteRenderer>();
        gun = GameObject.FindGameObjectWithTag("Gun").transform;
        renderGun = gun.GetComponent<SpriteRenderer>();
        point = gun.Find("Point");

        timer = 1f;
    }

    void Update()
    {
        if (Mathf.Abs(rb.velocity.x) > EPSILON)
            anim.SetBool("Is Running", true);
        else anim.SetBool("Is Running", false);

        if (Mathf.Abs(rb.velocity.y) > EPSILON)
            anim.SetBool("Is Jumping", true);
        else anim.SetBool("Is Jumping", false);

        if (rb.velocity.y < EPSILON * -1f)
            anim.SetBool("Is Descending", true);
        else anim.SetBool("Is Descending", false);

        if (sj.wallJump)
            anim.SetBool("Is Grinding", true);
        else anim.SetBool("Is Grinding", false);

        if (sj.touchesGround)
            anim.SetBool("Touches Ground", true);
        else anim.SetBool("Touches Ground", false);

        if (st.health <= 0)
        {
            body.enabled = true;
            anim.SetBool("Is Dead", true);
            sm.enabled = false;
            sj.enabled = false;
            ssh.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            if (st.isInvincible && Time.time - timer > flickerInterval)
            {
                body.enabled = !body.enabled;
                shoulder.enabled = !shoulder.enabled;
                back.enabled = !back.enabled;
                timer = Time.time;
            }
            else
            {
                body.enabled = true;
                shoulder.enabled = true;
                back.enabled = true;
            }
        }

        isMotionless = Time.time - st.timer < st.invincibility / st.motionLess;
        if (isMotionless)
        {
            anim.SetBool("Motionless", true);
            blood.enabled = true;
            sm.enabled = false;
            sj.enabled = false;
            if (sj.wallJump && rb.bodyType != RigidbodyType2D.Static)
                rb.velocity = new Vector2(0f, 0f);
        }
        else
        {
            anim.SetBool("Motionless", false);
            blood.enabled = false;
            sm.enabled = true;
            sj.enabled = true;

            Flip();
        }
    }

    void Flip()
    {
        if (sm.input > 0)
            flip = true;
        else if (sm.input < 0)
            flip = false;
        else return;

        if (sj.wallJump && Mathf.Abs(sm.input) > EPSILON)
            flip = !flip;

        renderGun.flipX = back.flipX = shoulder.flipX = body.flipX = !flip;

        Vector3 temp = point.localPosition;
        if ((flip && temp.x < 0) || (!flip && temp.x > 0))
            temp.x *= -1f;
        point.localPosition = temp;
    }
}
