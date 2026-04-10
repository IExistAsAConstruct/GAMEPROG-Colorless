using UnityEngine;

public class SludgeEnemy : EnemyBase
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float edgeCheckDistance = 1f;
    [SerializeField] private float edgeCheckOffset = 0.6f; // horizontal distance in front of center
    [SerializeField] private float wallCheckDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        float dir = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        Vector2 edgeRayOrigin = (Vector2)transform.position + Vector2.right * dir * edgeCheckOffset;

        bool groundAhead = Physics2D.Raycast(edgeRayOrigin, Vector2.down, edgeCheckDistance, groundLayer);
        bool wallAhead = Physics2D.Raycast(transform.position, Vector2.right * dir, wallCheckDistance, groundLayer);

        if (!groundAhead || wallAhead)
        {
            Flip();
        }
    }

    private void OnDrawGizmosSelected()
    {
        float dir = isFacingRight ? 1f : -1f;
        Vector2 edgeRayOrigin = (Vector2)transform.position + Vector2.right * dir * edgeCheckOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(edgeRayOrigin, edgeRayOrigin + Vector2.down * edgeCheckDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * dir * wallCheckDistance);
    }
}
