using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("Invincibility")]
    [SerializeField] private float iFrameDuration = 1f;
    [SerializeField] private float flashInterval = 0.1f;

    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsInvincible { get; private set; }

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private SpriteRenderer sr;
    private float iFrameTimer;
    private float flashTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        if (!IsInvincible) return;

        iFrameTimer -= Time.deltaTime;
        flashTimer -= Time.deltaTime;

        if (flashTimer <= 0f)
        {
            sr.enabled = !sr.enabled;
            flashTimer = flashInterval;
        }

        if (iFrameTimer <= 0f)
        {
            IsInvincible = false;
            sr.enabled = true;
        }
    }

    /// <summary>
    /// Compatibility wrapper: negative values deal damage, positive values heal.
    /// </summary>
    public void UpdateHealth(int amount)
    {
        if (amount < 0) TakeDamage(Mathf.Abs(amount));
        else Heal(amount);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || IsInvincible) return;

        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            IsDead = true;
            OnDeath?.Invoke();
            return;
        }

        IsInvincible = true;
        iFrameTimer = iFrameDuration;
        flashTimer = flashInterval;
    }

    public void Heal(int amount)
    {
        if (IsDead) return;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }
}