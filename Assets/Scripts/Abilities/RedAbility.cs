using UnityEngine;

public class RedAbility : ColorAbility
{
    [Header("Fireball")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireballCooldown = 0.6f;

    private float lastFireTime;

    public override bool OnBasicAttack()
    {
        // Red does a standard melee attack but can be modified here for more damage
        animator.SetTrigger("attack");
        playerAttack.DoMeleeHit(2f);
        return true;
    }

    public override void OnSpecialAbility()
    {
        if (Time.time < lastFireTime + fireballCooldown) return;
        if (fireballPrefab == null || firePoint == null) return;

        lastFireTime = Time.time;
        GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        if (fb.TryGetComponent<Fireball>(out var fireball))
        {
            // Requires IsFacingRight to be public in PlayerController
            Vector2 dir = playerController.IsFacingRight ? Vector2.right : Vector2.left;
            fireball.SetDirection(dir);
        }
    }
}