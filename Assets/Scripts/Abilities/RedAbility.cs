using UnityEngine;

public class RedAbility : ColorAbility
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