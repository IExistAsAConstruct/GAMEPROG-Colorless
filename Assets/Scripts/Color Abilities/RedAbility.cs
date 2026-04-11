using UnityEngine;

public class RedAbility : ColorAbility
{
    [Header("Fireball (Primary)")]
    public GameObject fireballPrefab;
    public float spawnDistance = 1.0f;

    [Header("Blast Jump (Secondary)")]
    public float blastJumpForce = 15f;

    private Rigidbody2D rb;
    private GameObject activeFireball;
    private bool canBlastJump = true;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!enabled) return;

        if (!canBlastJump && playerController.IsGrounded)
            canBlastJump = true;
    }

    public override void OnPrimary()
    {
        if (activeFireball != null) return;

        if (animator != null) animator.SetTrigger("attack");

        bool facingRight = playerController.IsFacingRight;
        Vector3 spawnDir = facingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + (spawnDir * spawnDistance);

        activeFireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);

        activeFireball.transform.localScale = new Vector3(facingRight ? 1f : -1f, 1f, 1f);

        Fireball fireball = activeFireball.GetComponent<Fireball>();
        if (fireball != null)
        {
            fireball.Launch(spawnDir);
        }
    }

    public override void OnSecondary()
    {
        if (!canBlastJump) return;

        canBlastJump = false;
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, blastJumpForce);
            if (animator != null) animator.SetTrigger("jump");
        }
    }
}