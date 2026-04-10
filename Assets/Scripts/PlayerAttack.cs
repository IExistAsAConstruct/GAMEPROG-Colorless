using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private ColorAbilityHandler colorHandler;
    private Animator anim;

    private void Awake()
    {
        colorHandler = GetComponent<ColorAbilityHandler>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        if (colorHandler != null && colorHandler.GetCurrentAbility() != null)
        {
            colorHandler.GetCurrentAbility().OnBasicAttack();
        }
    }

    public void OnAttackImpact()
    {
        if (colorHandler != null && colorHandler.GetCurrentAbility() != null)
        {
            colorHandler.GetCurrentAbility().OnSpecialAbility();
        }
    }
}