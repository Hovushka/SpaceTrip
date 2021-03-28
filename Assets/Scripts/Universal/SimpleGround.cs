using UnityEngine;

public class SimpleGround : MonoBehaviour
{
    public bool isGrounded = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
            isGrounded = true;
        if (collision.CompareTag("Player"))
            isGrounded = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
            isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Terrain"))
            isGrounded = false;
    }
}
