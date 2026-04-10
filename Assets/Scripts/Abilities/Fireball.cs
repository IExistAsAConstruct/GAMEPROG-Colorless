using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 2.5f;
    private Vector2 direction;

    void Start() => Destroy(gameObject, lifetime);

    void Update() => transform.Translate(direction * speed * Time.deltaTime);

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        if (dir == Vector2.left) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Health>(out var health))
        {
            health.UpdateHealth(-1);
            Destroy(gameObject);
        }
    }
}