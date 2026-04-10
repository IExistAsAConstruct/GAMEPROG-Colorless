using UnityEngine;

public class SludgeEnemy : EnemyBase
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float edgeCheckDistance = 1f;
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck; // place at the enemy's front foot

    private void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        // Move forward
        float dir = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        // Check for platform edge (no ground ahead)
        bool groundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, edgeCheckDistance, groundLayer);

        // Check for wall ahead
        bool wallAhead = Physics2D.Raycast(transform.position, Vector2.right * dir, wallCheckDistance, groundLayer);

        if (!groundAhead || wallAhead)
        {
            Flip();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * edgeCheckDistance);
        }
        float dir = isFacingRight ? 1f : -1f;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * dir * wallCheckDistance);
    }
}
