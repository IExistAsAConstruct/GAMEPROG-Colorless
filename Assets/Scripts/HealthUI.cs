using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays player health as heart icons (Zelda-style).
/// Subscribes to PlayerHealth.OnHealthChanged to update automatically.
/// 
/// Setup:
/// 1. Create a Canvas (Screen Space - Overlay)
/// 2. Add an empty GameObject as the heart container (with HorizontalLayoutGroup)
/// 3. Assign the container, full heart sprite, and empty heart sprite in the inspector
/// </summary>
public class HealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform heartContainer;

    [Header("Sprites")]
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Heart Size")]
    [SerializeField] private Vector2 heartSize = new Vector2(40f, 40f);

    private Image[] heartImages;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        InitializeHearts(playerHealth.CurrentHealth);
        playerHealth.OnHealthChanged += UpdateHearts;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHearts;
    }

    /// <summary>
    /// Creates heart icons as children of the container.
    /// </summary>
    private void InitializeHearts(int maxHealth)
    {
        // Clear any existing hearts
        foreach (Transform child in heartContainer)
            Destroy(child.gameObject);

        heartImages = new Image[maxHealth];

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heartObj = new GameObject($"Heart_{i}", typeof(Image));
            heartObj.transform.SetParent(heartContainer, false);

            Image img = heartObj.GetComponent<Image>();
            img.sprite = fullHeart;
            img.raycastTarget = false;

            // Set heart size
            RectTransform rect = heartObj.GetComponent<RectTransform>();
            rect.sizeDelta = heartSize;

            heartImages[i] = img;
        }
    }

    /// <summary>
    /// Called by PlayerHealth.OnHealthChanged event.
    /// Fills hearts left-to-right based on current health.
    /// </summary>
    private void UpdateHearts(int current, int max)
    {
        // Handle max health changes (e.g., health upgrades)
        if (heartImages == null || heartImages.Length != max)
            InitializeHearts(max);

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < current ? fullHeart : emptyHeart;
        }
    }
}