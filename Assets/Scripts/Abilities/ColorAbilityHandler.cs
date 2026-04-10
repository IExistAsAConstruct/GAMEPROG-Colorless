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

        if (ColorManager.Instance != null)
        {
            ColorManager.Instance.OnColorChanged += HandleColorChange;
        }
    }

    private void OnDestroy()
    {
        if (ColorManager.Instance != null)
        {
            ColorManager.Instance.OnColorChanged -= HandleColorChange;
        }
    }

    private void Update()
    {
        if (currentAbility != null)
        {
            if (Input.GetButtonDown("Fire1")) currentAbility.OnBasicAttack();
            if (Input.GetKeyDown(KeyCode.F)) currentAbility.OnSpecialAbility();
        }
    }

    private void HandleColorChange(ColorType newColor)
    {
        ColorAbility targetAbility = newColor switch
        {
            ColorType.Red => redAbility,
            ColorType.Blue => blueAbility,
            ColorType.Green => greenAbility,
            ColorType.Yellow => yellowAbility,
            _ => null
        };

        SwitchColor(targetAbility);
    }

    public void SwitchColor(ColorAbility newAbility)
    {
        if (currentAbility == newAbility) return;

        if (currentAbility != null) currentAbility.OnDeactivate();

        currentAbility = newAbility;

        if (currentAbility != null)
        {
            currentAbility.OnActivate();
            if (spriteRenderer != null) spriteRenderer.color = currentAbility.abilityColor;
        }
        else
        {
            if (spriteRenderer != null) spriteRenderer.color = Color.white;
        }
    }

    // ADD THIS LINE BELOW TO FIX THE ERRORS
    public ColorAbility GetCurrentAbility() => currentAbility;
}