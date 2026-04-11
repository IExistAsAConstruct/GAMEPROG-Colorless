using UnityEngine;

public class GreenSentry : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackInterval = 1.5f;
    public float detectionRadius = 3f;
    public int damage = 1;
    public LayerMask enemyLayer;

    [Header("Life Settings")]
    public float growTime = 1.0f;
    public float lifetime = 4f;

    private float nextAttackTime;
    private bool isGrown = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        Invoke(nameof(FinishGrowing), growTime);
    }

    private void FinishGrowing()
    {
        isGrown = true;
    }

    private void Update()
    {
        if (!isGrown) return;

        if (Time.time >= nextAttackTime)
        {
            AttackNearbyEnemies();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void AttackNearbyEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            var target = enemy.GetComponent<EnemyBase>();
            if (target != null) target.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}