using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Settings")]
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float attackRange = 0.8f;
    public float attackCooldown = 0.4f;

    private float lastAttackTime;
    private ColorAbilityHandler abilityHandler;
    private Animator animator;

    private void Awake()
    {
        abilityHandler = GetComponent<ColorAbilityHandler>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            abilityHandler?.CurrentAbility?.OnSpecialAbility();
        }
    }

    private void PerformAttack()
    {
        // Check if the current color has a special R-key move
        bool customHandled = abilityHandler?.CurrentAbility?.OnBasicAttack() ?? false;

        if (!customHandled)
        {
            animator.SetTrigger("attack");
            DoMeleeHit(1f);
        }
    }

    public void DoMeleeHit(float damage)
    {
        if (attackPoint == null) return;
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
                health.UpdateHealth(-(int)damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}