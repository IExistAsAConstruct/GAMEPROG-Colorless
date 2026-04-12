using UnityEngine;

public abstract class ColorAbility : MonoBehaviour
{
    public Color abilityColor;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public PlayerAttack playerAttack;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttack>();
        enabled = false;
    }

    public virtual void OnActivate() => enabled = true;
    public virtual void OnDeactivate() => enabled = false;

    public abstract void OnPrimary();
    public abstract void OnSecondary();

    public virtual void OnTertiary() { }
}