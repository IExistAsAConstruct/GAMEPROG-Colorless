using UnityEngine;
using System;

public enum PlayerColor { None, Red, Blue, Green, Yellow }

public class ColorSystem : MonoBehaviour
{
    public static ColorSystem Instance { get; private set; }

    [Header("State")]
    [SerializeField] private PlayerColor currentColor = PlayerColor.None;
    public PlayerColor CurrentColor => currentColor;

    [Header("Sprite Tinting")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color noneColor = Color.white;
    [SerializeField] private Color redColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private Color blueColor = new Color(0.3f, 0.5f, 1f);
    [SerializeField] private Color greenColor = new Color(0.3f, 1f, 0.3f);
    [SerializeField] private Color yellowColor = new Color(1f, 1f, 0.3f);

    private readonly PlayerColor[] colorCycle = {
        PlayerColor.None, PlayerColor.Red, PlayerColor.Blue,
        PlayerColor.Green, PlayerColor.Yellow
    };
    private int cycleIndex = 0;

    public event Action<PlayerColor> OnColorChanged;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        // Q/E cycling
        if (Input.GetKeyDown(KeyCode.Q)) CycleColor(-1);
        if (Input.GetKeyDown(KeyCode.E)) CycleColor(1);

        // 1-4 quick swap
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetColor(PlayerColor.Red);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetColor(PlayerColor.Blue);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetColor(PlayerColor.Green);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetColor(PlayerColor.Yellow);
    }

    private void CycleColor(int direction)
    {
        cycleIndex = (cycleIndex + direction + colorCycle.Length) % colorCycle.Length;
        SetColor(colorCycle[cycleIndex]);
    }

    public void SetColor(PlayerColor color)
    {
        if (currentColor == color) return;
        currentColor = color;
        cycleIndex = Array.IndexOf(colorCycle, color);
        ApplyTint(color);
        OnColorChanged?.Invoke(currentColor);
    }

    private void ApplyTint(PlayerColor color)
    {
        if (spriteRenderer == null) return;
        spriteRenderer.color = color switch
        {
            PlayerColor.Red => redColor,
            PlayerColor.Blue => blueColor,
            PlayerColor.Green => greenColor,
            PlayerColor.Yellow => yellowColor,
            _ => noneColor
        };
    }
}
