using UnityEngine;

public class RedAbility : ColorAbility
{
    public GameObject fireballPrefab;
    public float spawnDistance = 1.0f;
    public float blastJumpForce = 15f;

    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnPrimary()
    {
        if (animator != null) animator.SetTrigger("attack");
        Vector3 spawnDir = playerController.IsFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + (spawnDir * spawnDistance);
        GameObject projectile = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Fireball fireball = projectile.GetComponent<Fireball>();
        if (fireball != null) fireball.Launch(spawnDir);
    }

    public override void OnSecondary()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, blastJumpForce);
            if (animator != null) animator.SetTrigger("jump");
        }
    }
}