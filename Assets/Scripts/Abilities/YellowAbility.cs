using UnityEngine;

public class YellowAbility : ColorAbility
{
    [Header("Movement")]
    public float speedMultiplier = 1.5f;
    public float hoverFallSpeed = -0.5f;

    [Header("Combat")]
    public float damageRadius = 2f;
    public int contactDamage = 1;
    public float damageInterval = 0.5f;
    public LayerMask enemyLayer;

    private Rigidbody2D rb;
    private float nextDamageTime;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnActivate()
    {
        base.OnActivate();
        // Increase player speed when turning Yellow
        // Note: This assumes your PlayerController has a 'moveSpeed' variable
        GetComponent<PlayerController>().SetSpeedMultiplier(speedMultiplier);
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        // Reset speed when leaving Yellow
        GetComponent<PlayerController>().SetSpeedMultiplier(1f);
    }

    private void Update()
    {
        if (!enabled) return;

        HandleHover();
        HandleAoEDamage();
    }

    private void HandleHover()
    {
        if (Input.GetKey(KeyCode.Space) && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, hoverFallSpeed);
            animator.SetBool("isHovering", true);
        }
        else
        {
            animator.SetBool("isHovering", false);
        }
    }

    private void HandleAoEDamage()
    {
        if (Time.time >= nextDamageTime)
        {
            // Find all enemies in range and zap them
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, damageRadius, enemyLayer);
            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent<Health>(out var health))
                {
                    health.UpdateHealth(-contactDamage);
                }
            }
            nextDamageTime = Time.time + damageInterval;
        }
    }

    public override void OnSpecialAbility() { /* Reserved for extra effects */ }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}