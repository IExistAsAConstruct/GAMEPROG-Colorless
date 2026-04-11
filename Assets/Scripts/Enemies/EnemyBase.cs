using UnityEngine;

/// <summary>
/// Base class for all Colorless enemies. Handles health, damage, and common references.
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int contactDamage = 1;
    [SerializeField] protected float knockbackForce = 5f;

    protected int currentHealth;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected bool isFacingRight = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(contactDamage);
        }
    }
}
