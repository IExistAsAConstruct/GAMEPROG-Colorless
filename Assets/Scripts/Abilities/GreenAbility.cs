using UnityEngine;

public class GreenAbility : ColorAbility
{
    public override void OnBasicAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }
    }

    public override void OnSpecialAbility()
    {
    }
}