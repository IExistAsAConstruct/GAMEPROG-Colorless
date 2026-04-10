using UnityEngine;

public class ColorAbilityHandler : MonoBehaviour
{
    [Header("Abilities")]
    public ColorAbility redAbility;
    public ColorAbility blueAbility;
    public ColorAbility greenAbility;
    public ColorAbility yellowAbility;

    [Header("Override Controllers")]
    public RuntimeAnimatorController redOverride;
    public RuntimeAnimatorController blueOverride;
    public RuntimeAnimatorController greenOverride;
    public RuntimeAnimatorController yellowOverride;

    private ColorAbility currentAbility;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchColor(redAbility);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchColor(blueAbility);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchColor(greenAbility);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchColor(yellowAbility);

        if (Input.GetButtonDown("Fire1") && currentAbility != null)
        {
            currentAbility.OnBasicAttack();
        }
    }

    public void SwitchColor(ColorAbility newAbility)
    {
        if (newAbility == null || currentAbility == newAbility) return;

        if (currentAbility != null) currentAbility.OnDeactivate();

        currentAbility = newAbility;
        currentAbility.OnActivate();

        if (currentAbility == redAbility) anim.runtimeAnimatorController = redOverride;
        else if (currentAbility == blueAbility) anim.runtimeAnimatorController = blueOverride;
        else if (currentAbility == greenAbility) anim.runtimeAnimatorController = greenOverride;
        else if (currentAbility == yellowAbility) anim.runtimeAnimatorController = yellowOverride;

        if (spriteRenderer != null) spriteRenderer.color = currentAbility.abilityColor;
    }

    public ColorAbility GetCurrentAbility() => currentAbility;
}