using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsProvider : MonoBehaviour
{
    public Slider speedSlider;
    public Slider numYCellsSlider;
    public Slider pelletSnakeGrowthSlider;

    public static float cellsPerSecondMovement { get; private set; }
    public static int numYCells { get; private set; }
    public static int numXCells { get; private set; }
    public static int snakeGrowthTilesPerPellet { get; private set; }

    void Start()
    {
        speedSlider.onValueChanged.AddListener(OnSpeedChange);
        numYCellsSlider.onValueChanged.AddListener(OnNumYCellsChange);
        pelletSnakeGrowthSlider.onValueChanged.AddListener(OnPelletSnakeGrowthChange);
    }

    public void updateSettingValues() {
        OnSpeedChange(speedSlider.value);
        OnNumYCellsChange(numYCellsSlider.value);
        OnPelletSnakeGrowthChange(pelletSnakeGrowthSlider.value);
    }

    private void OnSpeedChange(float newValue)
    {
        cellsPerSecondMovement = newValue;
    }

    private void OnNumYCellsChange(float newValue)
    {
        numYCells = Mathf.RoundToInt(newValue);
        numXCells = Mathf.FloorToInt(numYCells * FindObjectOfType<Camera>().aspect);
    }

    private void OnPelletSnakeGrowthChange(float newValue)
    {
        snakeGrowthTilesPerPellet = Mathf.RoundToInt(newValue);
    }
}
