using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLightController : MonoBehaviour {

    public float degreesPerSecond;
    public float boostEffect;
    public float speedBoost;
    public float sunsetDegrees;

    private float rotateAmount;
    private float standardDownSpeed;
    private bool gameOver;
    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        standardDownSpeed = gameController.DOWN_SPEED;

        gameOver = false;
    }
	
	// Called each frame.
	void Update () {
        if (!gameOver)
        {
            rotateAmount = -1 * degreesPerSecond * (1 + (standardDownSpeed - gameController.DOWN_SPEED) / (1 / BoostEffectFunc(boostEffect) * standardDownSpeed));
            transform.Rotate(new Vector3(rotateAmount, 0.0f, 0.0f) * Time.deltaTime);

            if (300 < transform.eulerAngles.x && transform.eulerAngles.x < sunsetDegrees)
            {
                GameObject[] sendObjects;
                string[] tags = new string[] { "GameController", "MainCamera", "Light", "Player" };
                foreach (string tag in tags)
                {
                    sendObjects = GameObject.FindGameObjectsWithTag(tag);
                    foreach (GameObject sendObject in sendObjects)
                    {
                        sendObject.SendMessage("GameOver");
                    }
                }
            }
        }
    }

    void GameOver()
    {
        gameOver = true;
    }

    float BoostEffectFunc(float x)
    {
        return (1/(speedBoost - 1) * x + 1 / (speedBoost - 1));
    }
}
