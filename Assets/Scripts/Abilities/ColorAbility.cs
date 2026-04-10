using UnityEngine;

public abstract class ColorAbility : MonoBehaviour
{
    // This allows you to pick the color in the Inspector
    public Color abilityColor = Color.white;

    protected Animator animator;
    protected PlayerAttack playerAttack;
    protected PlayerController playerController;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerController = GetComponent<PlayerController>();
    }

    public virtual void OnActivate() { enabled = true; }
    public virtual void OnDeactivate() { enabled = false; }
    public virtual bool OnBasicAttack() { return false; }
    public abstract void OnSpecialAbility();
}