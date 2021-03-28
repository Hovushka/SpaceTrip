using UnityEngine;

public class SimpleSpikes : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<SimpleStats>().TakeDamage(50, gameObject, false);
    }
}
