using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private ColorAbilityHandler colorHandler;

    private void Awake()
    {
        colorHandler = GetComponent<ColorAbilityHandler>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (colorHandler != null && colorHandler.GetCurrentAbility() != null)
            {
                colorHandler.GetCurrentAbility().OnPrimary();
            }
        }
    }
}