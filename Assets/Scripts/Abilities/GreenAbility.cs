using UnityEngine;

public class GreenAbility : ColorAbility
{
    public GameObject healingPlantPrefab;
    public GameObject greenSentryPrefab;
    public int maxHealUses = 2;
    private int currentHealUses;
    public float spawnDistance = 1.5f;
    private PlayerHealth playerHealth;

    public override void OnActivate()
    {
        base.OnActivate();
        currentHealUses = maxHealUses;
        playerHealth = GetComponent<PlayerHealth>();
    }

    public override void OnPrimary()
    {
        if (animator != null) animator.SetTrigger("attack");
        Vector3 spawnDir = playerController.IsFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + (spawnDir * spawnDistance);
        spawnPos.y = transform.position.y;
        Instantiate(greenSentryPrefab, spawnPos, Quaternion.identity);
    }

    public override void OnSecondary()
    {
        if (currentHealUses > 0 && healingPlantPrefab != null)
        {
            currentHealUses--;
            if (playerHealth != null) playerHealth.UpdateHealth(1);
            if (animator != null) animator.SetTrigger("attack");
            GameObject plant = Instantiate(healingPlantPrefab, transform.position, Quaternion.identity);
            Destroy(plant, 1.5f);
        }
    }
}