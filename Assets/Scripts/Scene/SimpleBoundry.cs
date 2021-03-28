using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoundry : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject, 0f);
    }
}
