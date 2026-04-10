using UnityEngine;

public class GreenSentry : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackInterval = 1.5f;
    public float detectionRadius = 3f;
    public LayerMask enemyLayer;

    [Header("Life Settings")]
    public float growTime = 1.0f;

    private float nextAttackTime;
    private bool isGrown = false;

    void Start()
    {

        Destroy(gameObject, 4f);

        Invoke("FinishGrowing", growTime);
    }

    void FinishGrowing()
    {
        isGrown = true;
    }

    void Update()
    {
        if (!isGrown) return;

        if (Time.time >= nextAttackTime)
        {
            AttackNearbyEnemies();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    void AttackNearbyEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("Sentry attacking: " + enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}