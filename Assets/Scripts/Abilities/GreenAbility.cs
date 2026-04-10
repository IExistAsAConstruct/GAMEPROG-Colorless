using UnityEngine;

public class GreenAbility : ColorAbility
{
    [SerializeField] private GameObject sentryPrefab;
    [SerializeField] private GameObject healingPlantPrefab;

    public override bool OnBasicAttack()
    {
        // Spawns a Sentry when you press R (Basic Attack)
        Instantiate(sentryPrefab, transform.position + (transform.right * 1.5f), Quaternion.identity);
        return true;
    }

    public override void OnSpecialAbility()
    {
        // Spawns a Healing Plant when you press F (Special Ability)
        Instantiate(healingPlantPrefab, transform.position, Quaternion.identity);
    }
}