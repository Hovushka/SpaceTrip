using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWall : MonoBehaviour
{
    public bool isWalled = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Enemy")) && collision.gameObject.layer != 12)
            isWalled = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Enemy")) && collision.gameObject.layer != 12)
            isWalled = false;
    }
}
