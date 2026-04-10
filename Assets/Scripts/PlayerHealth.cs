using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (amount < 0)
        {
            Debug.Log(gameObject.name + " took damage! Health: " + currentHealth);
        }
        else if (amount > 0)
        {
            Debug.Log(gameObject.name + " healed! Health: " + currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " destroyed.");
        Destroy(gameObject);
    }
}