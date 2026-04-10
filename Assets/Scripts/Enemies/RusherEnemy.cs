using UnityEngine;

public class RusherEnemy : EnemyBase
{
    private enum State { Idle, WindUp, Dashing, Stunned, Recovering }

    [Header("Rush Behavior")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float windUpTime = 0.5f;
    [SerializeField] private float dashDuration = 0.8f;
    [SerializeField] private float recoverTime = 1.2f;
    [SerializeField] private float stunTime = 2f;

    private State state = State.Idle;
    private float stateTimer;
    private Transform player;
    private Vector2 dashDirection;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.WindUp:
                if (stateTimer <= 0f) BeginDash();
                break;
            case State.Dashing:
                Dash();
                if (stateTimer <= 0f) EnterState(State.Recovering, recoverTime);
                break;
            case State.Stunned:
                if (stateTimer <= 0f) EnterState(State.Recovering, recoverTime);
                break;
            case State.Recovering:
                if (stateTimer <= 0f) EnterState(State.Idle, 0f);
                break;
        }
    }

    private void IdleBehavior()
    {
        if (player == null) return;
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= detectionRange)
        {
            // Lock direction toward player
            dashDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;

            // Face the player
            if ((dashDirection.x > 0 && !isFacingRight) || (dashDirection.x < 0 && isFacingRight))
                Flip();

            EnterState(State.WindUp, windUpTime);
        }
    }

    private void BeginDash()
    {
        EnterState(State.Dashing, dashDuration);
    }

    private void Dash()
    {
        rb.linearVelocity = dashDirection * dashSpeed;
    }

    private void EnterState(State newState, float duration)
    {
        state = newState;
        stateTimer = duration;

        if (newState != State.Dashing)
            rb.linearVelocity = Vector2.zero;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == State.Dashing && collision.gameObject.CompareTag("IceWall"))
        {
            EnterState(State.Stunned, stunTime);
            return;
        }

        base.OnCollisionEnter2D(collision);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
