using UnityEngine;

public class ColorAbilityHandler : MonoBehaviour
{
    [Header("Ability Scripts")]
    public ColorAbility redAbility;
    public ColorAbility blueAbility;
    public ColorAbility greenAbility;
    public ColorAbility yellowAbility; // Added Yellow

    [Header("Animator Controllers")]
    public RuntimeAnimatorController redAnimator;
    public RuntimeAnimatorController blueAnimator;
    public RuntimeAnimatorController greenAnimator;
    public RuntimeAnimatorController yellowAnimator; // Added Yellow

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    public ColorAbility CurrentAbility { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (redAbility != null) SwitchColor(redAbility, redAnimator);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchColor(redAbility, redAnimator);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchColor(blueAbility, blueAnimator);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchColor(greenAbility, greenAnimator);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchColor(yellowAbility, yellowAnimator);
    }

    public void SwitchColor(ColorAbility newAbility, RuntimeAnimatorController newAnim)
    {
        if (newAbility == null) return;

        if (CurrentAbility != null) CurrentAbility.OnDeactivate();

        CurrentAbility = newAbility;
        CurrentAbility.OnActivate();

        spriteRenderer.color = newAbility.abilityColor;

        if (newAnim != null) anim.runtimeAnimatorController = newAnim;

        Debug.Log($"Switched to {newAbility.GetType().Name}");
    }
}