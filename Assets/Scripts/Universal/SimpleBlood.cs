using System.Collections;
using UnityEngine;

public class SimpleBlood : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sr.enabled)
            StartCoroutine("FadeOut");
        else
        {
            StopCoroutine("FadeOut");
            Color mew = sr.material.color;
            mew.a = 1f;
            sr.material.color = mew;
        }
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0f; f -= 0.045f)
        {
            Color mew = sr.material.color;
            mew.a = f;
            sr.material.color = mew;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
