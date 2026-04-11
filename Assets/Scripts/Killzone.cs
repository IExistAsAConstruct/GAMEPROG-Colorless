using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var health = collision.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(9999);
    }
}
