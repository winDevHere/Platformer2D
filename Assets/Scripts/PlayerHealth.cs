using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Vector3 spawnPoint;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
    }

    void Update()
    {
        if (transform.position.y < -10f) Die();
    }

    public void Die()
    {
        if (GameManager.Instance.IsGameOver) return;

        bool gameOver = GameManager.Instance.LoseLife();
        if (gameOver) return; // stay frozen, the panel takes over

        rb.linearVelocity = Vector2.zero;
        rb.position = spawnPoint;
        transform.position = spawnPoint;
    }
}