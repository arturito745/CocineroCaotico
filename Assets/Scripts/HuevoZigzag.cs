using UnityEngine;

public class HuevoZigzag : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 3f;
    private Vector2 moveDirection;
    private bool goingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("ChangeDirection", 0.5f, 0.5f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    void ChangeDirection()
    {
        moveDirection = goingRight ? new Vector2(1, 1).normalized : new Vector2(-1, 1).normalized;
        goingRight = !goingRight;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        goingRight = !goingRight;
        ChangeDirection();
    }
}
