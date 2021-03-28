using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleMovement : MonoBehaviour
{
    public float movementSpeed = 60f;
    public float airDegrade = 0.9f;

    [HideInInspector] public float input;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        input = Input.GetAxis("Horizontal");
        float actualDegrade = (Mathf.Abs(rb.velocity.y) > 0) ? airDegrade : 1f;

        rb.velocity = new Vector2(input * movementSpeed * actualDegrade, rb.velocity.y);
    }
}
