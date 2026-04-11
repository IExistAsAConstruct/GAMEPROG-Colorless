using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    public float lifetime = 3f;

    private Vector2 direction;
    private HashSet<Collider2D> alreadyHit = new HashSet<Collider2D>();

    public void Launch(Vector2 launchDirection)
    {
        direction = launchDirection;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;
        if (collision.gameObject.name.Contains("Fireball")) return;

        // Destroy on terrain
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        // Pierce through enemies, damage each once
        var enemy = collision.GetComponent<EnemyBase>();
        if (enemy != null && !alreadyHit.Contains(collision))
        {
            alreadyHit.Add(collision);
            enemy.TakeDamage(damage);
        }
    }
}