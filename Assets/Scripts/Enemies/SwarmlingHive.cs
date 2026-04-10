using UnityEngine;

public class SwarmlingHive : MonoBehaviour
{
    [Header("Chase Area")]
    [SerializeField] private float chaseRadius = 8f;
    public float ChaseRadius => chaseRadius;

    [Header("Spawning")]
    [SerializeField] private GameObject swarmlingPrefab;
    [SerializeField] private int maxSwarmlings = 6;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnRadius = 1.5f;

    private int aliveCount;
    private float spawnTimer;

    private void Start()
    {
        for (int i = 0; i < maxSwarmlings; i++)
            SpawnSwarmling();
    }

    private void Update()
    {
        if (aliveCount >= maxSwarmlings) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnSwarmling();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnSwarmling()
    {
        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + (Vector3)offset;

        var obj = Instantiate(swarmlingPrefab, spawnPos, Quaternion.identity);
        var swarmling = obj.GetComponent<SwarmlingEnemy>();
        swarmling.Initialize(this);

        aliveCount++;
    }

    public void OnSwarmlingDied()
    {
        aliveCount--;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
