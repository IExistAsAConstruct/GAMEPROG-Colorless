using UnityEngine;

public class SludgeBall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"SludgeBall collided with {collision.gameObject.name}", this);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(1);
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }
}
