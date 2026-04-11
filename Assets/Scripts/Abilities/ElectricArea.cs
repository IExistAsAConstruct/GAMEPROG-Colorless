using UnityEngine;

public class ElectricArea : MonoBehaviour
{
    public int damage = 1;
    public float damageInterval = 0.5f;
    private float nextDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;
        if (Time.time < nextDamageTime) return;

        var target = collision.GetComponent<EnemyBase>();
        if (target != null)
        {
            target.TakeDamage(damage);
            nextDamageTime = Time.time + damageInterval;
        }
    }
}