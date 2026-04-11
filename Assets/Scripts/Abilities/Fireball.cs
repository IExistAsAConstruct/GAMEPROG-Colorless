using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifetime = 3f;

    private Vector2 moveDirection;
    private HashSet<Collider2D> alreadyHit = new HashSet<Collider2D>();

    public void Launch(Vector2 launchDirection)
    {
        moveDirection = launchDirection;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.gameObject.name.Contains("Fireball"))
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        if (collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            if (!alreadyHit.Contains(collision))
            {
                alreadyHit.Add(collision);
                enemy.TakeDamage(damage);
            }
        }
    }
}