using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsProvider : MonoBehaviour
{
    public string settingsSceneName;

    public static float cellsPerSecondMovement { get; private set; }
    public static int numYCells { get; private set; }
    public static int numXCells { get; private set; }
    public static int snakeGrowthTilesPerPellet { get; private set; }
    private Slider speedSlider;
    private Slider numYCellsSlider;
    private Slider pelletSnakeGrowthSlider;

    void Start()
    {
        findSliders();
        SceneManager.activeSceneChanged += OnSceneChange;
        speedSlider.onValueChanged.AddListener(OnSpeedChange);
        numYCellsSlider.onValueChanged.AddListener(OnNumYCellsChange);
        pelletSnakeGrowthSlider.onValueChanged.AddListener(OnPelletSnakeGrowthChange);
    }

    public void updateSettingValues() {
        OnSpeedChange(speedSlider.value);
        OnNumYCellsChange(numYCellsSlider.value);
        OnPelletSnakeGrowthChange(pelletSnakeGrowthSlider.value);
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.name.Equals(settingsSceneName)) {
            findSliders();
        }
    }

    private void findSliders()
    {
        speedSlider = GameObject.FindGameObjectWithTag("SpeedSlider").GetComponent<Slider>();
        numYCellsSlider = GameObject.FindGameObjectWithTag("MapSizeSlider").GetComponent<Slider>();
        pelletSnakeGrowthSlider = GameObject.FindGameObjectWithTag("SnakeGrowthSlider").GetComponent<Slider>();
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
