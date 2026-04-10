using UnityEngine;

public class ColorAbilityHandler : MonoBehaviour
{
    [Header("Abilities")]
    public ColorAbility redAbility;
    public ColorAbility blueAbility;
    public ColorAbility greenAbility;
    public ColorAbility yellowAbility;

    private ColorAbility currentAbility;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchColor(redAbility);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchColor(blueAbility);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchColor(greenAbility);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchColor(yellowAbility);

        if (currentAbility != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                currentAbility.OnBasicAttack();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                currentAbility.OnSpecialAbility();
            }
        }
    }

    public void SwitchColor(ColorAbility newAbility)
    {
        if (newAbility == null || currentAbility == newAbility) return;

        if (currentAbility != null) currentAbility.OnDeactivate();

        currentAbility = newAbility;
        currentAbility.OnActivate();

        if (spriteRenderer != null) spriteRenderer.color = currentAbility.abilityColor;
    }

    public ColorAbility GetCurrentAbility() => currentAbility;
}