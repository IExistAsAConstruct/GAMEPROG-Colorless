using UnityEngine;

public class GreenAbility : ColorAbility
{
    [Header("Sentry (Primary)")]
    public GameObject greenSentryPrefab;
    public float spawnDistance = 1.5f;

    [Header("Vine (Secondary)")]
    public GameObject vinePrefab;
    public float vineInteractRange = 3f;

    public override void OnPrimary()
    {
        if (greenSentryPrefab == null) return;
        if (animator != null) animator.SetTrigger("attack");

        Vector3 spawnDir = playerController.IsFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + (spawnDir * spawnDistance);
        spawnPos.y = transform.position.y;

        Instantiate(greenSentryPrefab, spawnPos, Quaternion.identity);
    }

    public override void OnSecondary()
    {
        if (vinePrefab == null) return;
        if (animator != null) animator.SetTrigger("attack");

        // Find the nearest Planter within range
        Planter[] planters = FindObjectsByType<Planter>(FindObjectsSortMode.None);
        Planter nearest = null;
        float nearestDist = vineInteractRange;

        foreach (var p in planters)
        {
            if (p.HasVine) continue;
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = p;
            }
        }

        if (nearest != null)
            nearest.GrowVine(vinePrefab);
    }
}