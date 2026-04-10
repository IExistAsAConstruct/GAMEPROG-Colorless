using UnityEngine;

public class SwarmlingEnemy : EnemyBase
{
    [Header("Swarm Behavior")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float wobbleStrength = 0.5f;

    private Transform player;
    private SwarmlingHive hive;
    private float wobbleOffset;

    protected override void Awake()
    {
        base.Awake();
        maxHealth = 1;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        wobbleOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    public void Initialize(SwarmlingHive sourceHive)
    {
        hive = sourceHive;
    }

    private void FixedUpdate()
    {
        if (player == null || hive == null) return;

        float distToHive = Vector2.Distance(player.position, hive.transform.position);
        if (distToHive > hive.ChaseRadius) return;

        Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float wobble = Mathf.Sin(Time.time * 3f + wobbleOffset) * wobbleStrength;
        Vector2 perpendicular = new Vector2(-dir.y, dir.x);

        rb.linearVelocity = (dir * moveSpeed) + (perpendicular * wobble);
    }

    protected override void Die()
    {
        if (hive != null) hive.OnSwarmlingDied();
        base.Die();
    }
}
