using UnityEngine;

public class SimpleGranade : SimpleArmory
{
    public GameObject granade;
    public Vector2 throwForce;

    private Animator anim;
    private SimpleAnimator sa;
    private Transform point;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        sa = gameObject.GetComponentInParent<SimpleAnimator>();
        point = transform.Find("Point");
        point.localPosition = new Vector3(1.5f, 10.5f, 0f);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            anim.SetBool("Commit Throw", true);
        else anim.SetBool("Commit Throw", false);

        if (sa.isMotionless)
            anim.SetBool("Got Staggered", true);
        else anim.SetBool("Got Staggered", false);
    }

    public void Throw()
    {
        float angle = (point.localPosition.x > 0) ? 1f : -1f;
        GameObject projectile = Instantiate<GameObject>(granade);
        projectile.transform.position = point.transform.position;
        projectile.GetComponent<Rigidbody2D>().AddForce(throwForce * Mathf.Abs(Physics2D.gravity.y)); 
    }
}
