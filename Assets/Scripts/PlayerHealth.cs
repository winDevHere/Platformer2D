using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Enemy hit")]
    [SerializeField] float knockbackSpeed = 7f;                        // horizontal push away from the enemy
    [SerializeField] float knockbackDuration = 0.25f;                  // movement input locked
    [SerializeField] float invulnerableDuration = 1f;                  // can't be hit again
    [SerializeField] float flickerInterval = 0.1f;

    Vector3 spawnPoint;
    Rigidbody2D rb;
    PlayerController controller;
    SpriteRenderer sprite;
    bool invulnerable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (transform.position.y < -10f) Die();
    }

    // Hit by an enemy: lose a life and get knocked away from it instead of respawning.
    public void TakeHit(Vector2 sourcePosition)
    {
        if (GameManager.Instance.IsGameOver || invulnerable) return;

        bool gameOver = GameManager.Instance.LoseLife();
        if (gameOver) return; // stay frozen, the panel takes over

        float away = transform.position.x >= sourcePosition.x ? 1f : -1f;
        controller.ApplyKnockback(away * knockbackSpeed, knockbackDuration);
        StartCoroutine(InvulnerabilityRoutine());
    }

    // Fell off the level: lose a life and respawn at the start.
    public void Die()
    {
        if (GameManager.Instance.IsGameOver) return;

        bool gameOver = GameManager.Instance.LoseLife();
        if (gameOver) return; // stay frozen, the panel takes over

        rb.linearVelocity = Vector2.zero;
        rb.position = spawnPoint;
        transform.position = spawnPoint;
    }

    IEnumerator InvulnerabilityRoutine()
    {
        invulnerable = true;

        for (float t = 0f; t < invulnerableDuration; t += flickerInterval)
        {
            if (sprite != null) sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(flickerInterval);
        }

        if (sprite != null) sprite.enabled = true;
        invulnerable = false;
    }
}
