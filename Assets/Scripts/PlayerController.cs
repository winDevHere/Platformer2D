using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 12f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius = 0.15f;
    [SerializeField] LayerMask groundLayer;

    Rigidbody2D rb;
    float moveInput;
    bool isGrounded;
    float knockbackTimer;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    // Pushes the player horizontally (vertical velocity is left untouched) and ignores
    // movement input for `duration` seconds so FixedUpdate doesn't overwrite the push.
    public void ApplyKnockback(float xVelocity, float duration)
    {
        rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        knockbackTimer = duration;
        moveInput = 0f;
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (knockbackTimer > 0f) return; // no steering or jumping while stunned

        moveInput = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) moveInput -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) moveInput += 1f;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (kb.spaceKey.wasPressedThisFrame && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}