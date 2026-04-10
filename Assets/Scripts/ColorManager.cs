using UnityEngine;
using System;
using System.Collections.Generic;

public class ColorManager : Singleton<ColorManager>
{
    public ColorType CurrentColor { get; private set; } = ColorType.White;
    public event Action<ColorType> OnColorChanged;

    private bool hasLeftWhite = false;
    private readonly List<ColorType> colorCycle = new List<ColorType>
        { ColorType.Red, ColorType.Blue, ColorType.Green, ColorType.Yellow };
    private int cycleIndex = 0;

    private void Update()
    {
        // Q/E to Cycle
        if (Input.GetKeyDown(KeyCode.Q)) CycleColor(-1);
        else if (Input.GetKeyDown(KeyCode.E)) CycleColor(1);

        // 1-4 for Direct Pick
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
        OnColorChanged?.Invoke(CurrentColor);
    }
}