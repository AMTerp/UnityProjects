using System;
using System.Collections;
using System.Collections.Generic;
using Snake.Grid;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GridController gridController;

    void Start()
    {
        gridController = FindObjectOfType<GridController>();
        initializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initializeGame()
    {
        // initializeScore();
        initializePlayer();
    }

    private void initializePlayer()
    {
        
    }
}
