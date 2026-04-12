using UnityEngine;

public class GoliathEnemy : EnemyBase
{
    [Header("Edge Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float edgeCheckDistance = 1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float aggroRange = 12f;

    [Header("Slam Attack")]
    [SerializeField] private float slamRange = 3f;
    [SerializeField] private float slamCooldown = 2f;
    [SerializeField] private float slamWindUp = 0.6f;
    [SerializeField] private int slamDamage = 2;
    [SerializeField] private float slamRadius = 2.5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Sludge Lob")]
    [SerializeField] private float lobRange = 10f;
    [SerializeField] private float lobCooldown = 3f;
    [SerializeField] private GameObject sludgeBallPrefab;
    [SerializeField] private Transform lobPoint;
    [SerializeField] private float lobArcHeight = 5f;

    private Transform player;
    private float slamTimer;
    private float lobTimer;
    private bool isSlamming;
    private float slamWindUpTimer;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        slamTimer -= Time.deltaTime;
        lobTimer -= Time.deltaTime;

        if (isSlamming)
        {
            slamWindUpTimer -= Time.deltaTime;
            if (slamWindUpTimer <= 0f) ExecuteSlam();
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > aggroRange) return;

        if ((player.position.x > transform.position.x && !isFacingRight) ||
            (player.position.x < transform.position.x && isFacingRight))
            Flip();

        if (dist <= slamRange && slamTimer <= 0f)
        {
            StartSlam();
        }
        else if (dist <= lobRange && dist > slamRange && lobTimer <= 0f)
        {
            LobSludge();
        }
        else
        {
            float dir = player.position.x > transform.position.x ? 1f : -1f;

            // Stop at ledges
            bool groundAhead = groundCheck != null
                && Physics2D.Raycast(groundCheck.position, Vector2.down, edgeCheckDistance, groundLayer);

            if (groundAhead)
                rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    private void StartSlam()
    {
        isSlamming = true;
        slamWindUpTimer = slamWindUp;
        rb.linearVelocity = Vector2.zero;
        if (animator != null) animator.SetBool("isSlamming", true);
    }

    private void ExecuteSlam()
    {
        isSlamming = false;
        slamTimer = slamCooldown;
        if (animator != null) animator.SetBool("isSlamming", false);

        Collider2D hit = Physics2D.OverlapCircle(transform.position, slamRadius, playerLayer);
        if (hit != null && hit.CompareTag("Player"))
        {
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null) health.UpdateHealth(-slamDamage);

            var hitRb = hit.GetComponent<Rigidbody2D>();
            if (hitRb != null)
            {
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                hitRb.AddForce((dir + Vector2.up) * knockbackForce * 1.5f, ForceMode2D.Impulse);
            }
        }
    }

    private void LobSludge()
    {
        lobTimer = lobCooldown;

        if (sludgeBallPrefab == null || lobPoint == null) return;

        if (animator != null) animator.SetBool("isThrowing", true);

        var ball = Instantiate(sludgeBallPrefab, lobPoint.position, Quaternion.identity);
        var ballRb = ball.GetComponent<Rigidbody2D>();

        Vector2 target = player.position;
        Vector2 origin = lobPoint.position;
        Vector2 velocity = CalculateArcVelocity(origin, target, lobArcHeight);
        ballRb.linearVelocity = velocity;

        Destroy(ball, 5f);

        Invoke(nameof(EndThrow), 0.5f);
    }

    private void EndThrow()
    {
        if (animator != null) animator.SetBool("isThrowing", false);
    }

    private Vector2 CalculateArcVelocity(Vector2 origin, Vector2 target, float arcHeight)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float peakY = Mathf.Max(origin.y, target.y) + arcHeight;

        float vy = Mathf.Sqrt(2f * gravity * (peakY - origin.y));
        float totalTime = (vy + Mathf.Sqrt(Mathf.Max(0f, vy * vy - 2f * gravity * (origin.y - target.y)))) / gravity;
        float vx = (target.x - origin.x) / totalTime;

        return new Vector2(vx, vy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRange);
        Gizmos.color = new Color(0.5f, 0f, 0.5f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, slamRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lobRange);

        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * edgeCheckDistance);
        }
    }
}
