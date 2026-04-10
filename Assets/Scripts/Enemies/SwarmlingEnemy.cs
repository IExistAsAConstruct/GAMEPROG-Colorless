using UnityEngine;

public class SwarmlingEnemy : EnemyBase
{
    [Header("Swarm Behavior")]
    [SerializeField] private float chaseRadius = 8f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float wobbleStrength = 0.5f; // organic movement
    [SerializeField] private int contactDmg = 1;

    private Transform player;
    private float wobbleOffset;

    protected override void Awake()
    {
        base.Awake();
        maxHealth = 1; // dies in one hit
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        wobbleOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > chaseRadius) return;

        // Drift toward player with a slight sine wobble for organic feel
        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float wobble = Mathf.Sin(Time.time * 3f + wobbleOffset) * wobbleStrength;
        Vector2 perpendicular = new Vector2(-dir.y, dir.x);

        rb.linearVelocity = (dir * moveSpeed) + (perpendicular * wobble);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
