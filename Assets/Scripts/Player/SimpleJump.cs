using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SimpleMovement))]
public class SimpleJump : MonoBehaviour
{
    public float ascendSpeed = 20f;
    public float descendAcceleration = 100f;
    public float jumpLength = 2f;
    public float bufferLength = 0.5f;
    public float initialAcceleration = 200f;
    public float wallKickbackX = 400f;
    public float wallKickbackY = 800f;
    //public float wallLengthDegrade = 0.5f;
    public float wallGrind = 0.5f;

    [HideInInspector] public bool wallJump = false;
    [HideInInspector] public bool ascending = false;
    [HideInInspector] public bool touchesGround = false;

    private Rigidbody2D rb;
    private SimpleMovement sm;
    private SpriteRenderer body;
    private bool canJump = false;
    private readonly float EPSILON = 0.01f;
    private float timer = -1f;
    private float buffer = -1f;
    private bool headBump = false;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sm = gameObject.GetComponent<SimpleMovement>();
        body = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (canJump && Time.time - buffer < bufferLength)
        {
            ascending = true;
            timer = Time.time;
            buffer = -1f;
            rb.AddForce(new Vector2(0f, initialAcceleration));

            if (wallJump)
            {
                timer = -1f;
                rb.velocity = new Vector2(0, 0);
                float wallJumpWhere = 1f;
                if (!body.flipX)
                    wallJumpWhere = -1f;
                rb.AddForce(new Vector2(wallKickbackX * Physics2D.gravity.y * wallJumpWhere, wallKickbackY * Mathf.Abs(Physics2D.gravity.y)));
            }
        }

        if ((touchesGround && !ascending && Mathf.Abs(rb.velocity.y) < EPSILON) || (wallJump && Mathf.Abs(sm.input) > EPSILON))
            canJump = true;
        else canJump = false;

        if (ascending && Input.GetButton("Jump") && Time.time - timer <= jumpLength && !headBump)
            rb.velocity = new Vector2(rb.velocity.x, ascendSpeed);
        else
        {
            ascending = false;
            rb.AddForce(new Vector2(0f, Physics2D.gravity.y * descendAcceleration));
        }

        if (Input.GetButtonDown("Jump"))
            buffer = Time.time;

        if (rb.velocity.y < 0 && wallJump && Mathf.Abs(sm.input) > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallGrind);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.CompareTag("Player"))
            wallJump = true;
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.name == "Feet")
            touchesGround = true;
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.name == "Head")
            headBump = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.CompareTag("Player"))
            wallJump = true;
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.name == "Feet")
            touchesGround = true;
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.name == "Head")
            headBump = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.CompareTag("Player"))
            wallJump = false;
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.name == "Feet")
            touchesGround = false;
        if (collision.gameObject.tag.Equals("Terrain") && collision.otherCollider.name == "Head")
            headBump = false;
    }
}