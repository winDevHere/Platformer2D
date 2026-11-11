using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] Transform ledgeCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float checkRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    int direction = 1;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        bool groundAhead = Physics2D.OverlapCircle(ledgeCheck.position, checkRadius, groundLayer);
        bool wallAhead = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (!groundAhead || wallAhead)
            Flip();

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    void Flip()
    {
        direction *= -1;
        // Mirror the child check points to the other side
        var s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Player")) return;

        // Stomp: the player hit the enemy from above
        bool stomped = col.GetContact(0).normal.y < -0.5f;

        if (stomped)
        {
            col.collider.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0f, 8f); // bounce
            Destroy(gameObject);
        }
        else
        {
            col.collider.GetComponent<PlayerHealth>()?.Die();
        }
    }
}