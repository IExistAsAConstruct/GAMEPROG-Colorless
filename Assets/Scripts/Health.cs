using UnityEngine;

public class Health : MonoBehaviour
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

        if (amount < 0)
        {
            // Optional: Play a "get hit" animation here
            Debug.Log(gameObject.name + " took damage! Health: " + currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // No ScoreManager call here! 
        // Just destroy the object or play a death animation.
        Debug.Log(gameObject.name + " destroyed.");
        Destroy(gameObject);
    }
}