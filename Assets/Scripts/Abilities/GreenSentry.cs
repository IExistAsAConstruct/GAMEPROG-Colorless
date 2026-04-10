using UnityEngine;

public class GreenSentry : MonoBehaviour
{
    [Header("Combat Settings")]
    public float searchRange = 4f;
    public float fireRate = 1f;
    public LayerMask targetLayer;

    private float nextFireTime;

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FindAndDamageTarget();
        }
    }

    private void FindAndDamageTarget()
    {
        // Search for a collider on the specific layer
        Collider2D target = Physics2D.OverlapCircle(transform.position, searchRange, targetLayer);

        if (target != null)
        {
            // If the target has a Health script, hit it
            if (target.TryGetComponent<Health>(out var health))
            {
                health.UpdateHealth(-1);
                nextFireTime = Time.time + fireRate;
                Debug.Log("Sentry hit: " + target.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }
}