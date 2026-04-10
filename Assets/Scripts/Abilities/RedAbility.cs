using UnityEngine;

public class RedAbility : ColorAbility
{
    public GameObject fireballPrefab;
    public float spawnDistance = 1.0f;

    public override void OnBasicAttack()
    {
        if (animator != null) animator.SetTrigger("attack");

        Vector3 spawnDir = playerController.IsFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + (spawnDir * spawnDistance);

        GameObject projectile = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Fireball fireball = projectile.GetComponent<Fireball>();

        if (fireball != null)
        {
            fireball.Launch(spawnDir);
        }
    }

    public override void OnSpecialAbility()
    {
        Debug.Log("Red Special Ability Activated!");
    }
}