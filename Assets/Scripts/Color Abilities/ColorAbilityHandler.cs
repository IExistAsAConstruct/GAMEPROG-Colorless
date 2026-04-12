using UnityEngine;

public class ColorAbilityHandler : MonoBehaviour
{
    [Header("Abilities")]
    public ColorAbility redAbility;
    public ColorAbility blueAbility;
    public ColorAbility greenAbility;
    public ColorAbility yellowAbility;

    private ColorAbility currentAbility;

    private void Start()
    {
        if (ColorManager.Instance != null)
            ColorManager.Instance.OnColorChanged += HandleColorChange;
    }

    private void OnDestroy()
    {
        if (ColorManager.Instance != null)
            ColorManager.Instance.OnColorChanged -= HandleColorChange;
    }

    private void Update()
    {
        if (currentAbility != null)
        {
            if (Input.GetKeyDown(KeyCode.F)) currentAbility.OnPrimary();
            if (Input.GetKeyDown(KeyCode.R)) currentAbility.OnSecondary();
            if (Input.GetKeyDown(KeyCode.V)) currentAbility.OnTertiary();
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
            currentAbility.OnActivate();
    }

    public ColorAbility GetCurrentAbility() => currentAbility;
}