using UnityEngine;

public class YellowAbility : ColorAbility
{
    [Header("Yellow Settings")]
    public float speedMultiplier = 1.5f;
    public float hoverFallSpeed = -0.8f;

    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnActivate()
    {
        base.OnActivate();
        if (playerController != null)
        {
            playerController.SetSpeedMultiplier(speedMultiplier);
        }
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        if (playerController != null)
        {
            playerController.SetSpeedMultiplier(1f);
        }
        if (animator != null)
        {
            animator.SetBool("isHovering", false);
        }
    }

    private void Update()
    {
        if (!enabled) return;

        if (Input.GetKey(KeyCode.Space) && rb != null && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, hoverFallSpeed);
            if (animator != null) animator.SetBool("isHovering", true);
        }
        else
        {
            if (animator != null) animator.SetBool("isHovering", false);
        }
    }

    public override void OnBasicAttack()
    {
        if (animator != null) animator.SetTrigger("attack");
    }

    public override void OnSpecialAbility()
    {
    }
}