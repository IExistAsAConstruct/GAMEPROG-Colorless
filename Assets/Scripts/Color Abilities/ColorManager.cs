using UnityEngine;
using System;
using System.Collections.Generic;

public class ColorManager : Singleton<ColorManager>
{
    public ColorType CurrentColor { get; private set; } = ColorType.White;
    public event Action<ColorType> OnColorChanged;

    [Header("Sprite Tinting")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Color whiteColor = Color.white;
    [SerializeField] private Color redColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private Color blueColor = new Color(0.3f, 0.5f, 1f);
    [SerializeField] private Color greenColor = new Color(0.3f, 1f, 0.3f);
    [SerializeField] private Color yellowColor = new Color(1f, 1f, 0.3f);

    private bool hasLeftWhite = false;
    private readonly List<ColorType> colorCycle = new List<ColorType>
        { ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow };
    private int cycleIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) CycleColor(-1);
        else if (Input.GetKeyDown(KeyCode.E)) CycleColor(1);

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwapColor(ColorType.Red);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwapColor(ColorType.Blue);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwapColor(ColorType.Green);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SwapColor(ColorType.Yellow);
    }

    private void CycleColor(int direction)
    {
        if (!hasLeftWhite) cycleIndex = direction >= 0 ? 0 : colorCycle.Count - 1;
        else cycleIndex = (cycleIndex + direction + colorCycle.Count) % colorCycle.Count;

        SwapColor(colorCycle[cycleIndex]);
    }

    public void SwapColor(ColorType newColor)
    {
        if (newColor == ColorType.White && hasLeftWhite) return;
        if (CurrentColor == newColor) return;

        if (newColor != ColorType.White)
        {
            hasLeftWhite = true;
            int idx = colorCycle.IndexOf(newColor);
            if (idx >= 0) cycleIndex = idx;
        }

        CurrentColor = newColor;
        ApplyTint(newColor);
        OnColorChanged?.Invoke(CurrentColor);
    }

    private void ApplyTint(ColorType color)
    {
        if (playerSprite == null) return;
        playerSprite.color = color switch
        {
            ColorType.Red => redColor,
            ColorType.Blue => blueColor,
            ColorType.Green => greenColor,
            ColorType.Yellow => yellowColor,
            _ => whiteColor
        };
    }
}