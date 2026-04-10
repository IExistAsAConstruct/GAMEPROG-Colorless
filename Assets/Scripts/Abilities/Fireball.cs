using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 1;
    public float lifetime = 3f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Launch(bool facingRight)
    {
        float direction = facingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * speed, 0);

        if (!facingRight)
        {
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.UpdateHealth(-damage);
            }
            Explode();
        }
        else if (collision.CompareTag("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}