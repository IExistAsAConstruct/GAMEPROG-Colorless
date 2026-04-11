using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all Colorless enemies. Handles health, damage, and common references.
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int contactDamage = 1;
    [SerializeField] protected float knockbackForce = 5f;

    [Header("Damage Feedback")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashDuration = 0.15f;

    protected int currentHealth;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected bool isFacingRight = true;

    private Color originalColor;
    private Coroutine flashRoutine;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        if (sr != null) originalColor = sr.color;

        ValidateSetup();
    }

    public virtual void TakeDamage(int damage)
    {
        Debug.Log($"{name}: took {damage} damage, health {currentHealth} -> {currentHealth - damage}");
        currentHealth -= damage;

        // Visual feedback
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(DamageFlash());

        if (currentHealth <= 0) Die();
    }

    private IEnumerator DamageFlash()
    {
        if (sr != null)
        {
            sr.color = hitColor;
            yield return new WaitForSeconds(hitFlashDuration);
            sr.color = originalColor;
        }
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
        if (!collision.gameObject.CompareTag("Player")) return;

        var playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            playerRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }

        collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(contactDamage);
    }

    private void ValidateSetup()
    {
        if (rb == null) Debug.LogWarning($"{name}: Missing Rigidbody2D", this);
        if (sr == null) Debug.LogWarning($"{name}: Missing SpriteRenderer", this);
        if (GetComponent<Collider2D>() == null) Debug.LogWarning($"{name}: Missing Collider2D", this);
        if (gameObject.layer == 0) Debug.LogWarning($"{name}: Still on Default layer - set to Enemy", this);
    }
}
