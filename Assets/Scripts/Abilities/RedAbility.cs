using UnityEngine;

public class RedAbility : ColorAbility
{
    public GameObject fireballPrefab;
    public Transform firePoint;

    public override void OnBasicAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("attack");
        }
    }

    public override void OnSpecialAbility()
    {
        Debug.Log("Red Special Activated!");
        if (fireballPrefab != null && firePoint != null)
        {
            GameObject ball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
            Fireball script = ball.GetComponent<Fireball>();
            if (script != null)
            {
                script.Launch(playerController.IsFacingRight);
            }
        }
        else
        {
            Debug.LogError("Fireball Prefab or Fire Point is missing in the Inspector!");
        }
    }
}