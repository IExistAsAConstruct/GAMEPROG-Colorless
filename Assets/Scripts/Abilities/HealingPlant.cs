using UnityEngine;

public class SentryPlant : MonoBehaviour
{
    public float range = 4f;
    public LayerMask enemyLayer;
    public float fireRate = 1f;
    private float nextFire;

    void Update()
    {
        if (Time.time > nextFire)
        {
            Collider2D enemy = Physics2D.OverlapCircle(transform.position, range, enemyLayer);
            if (enemy != null)
            {
                if (enemy.TryGetComponent<PlayerHealth>(out var h)) h.UpdateHealth(-1);
                nextFire = Time.time + fireRate;
            }
        }
    }
}