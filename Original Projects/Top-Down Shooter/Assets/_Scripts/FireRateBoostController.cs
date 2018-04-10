using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateBoostController : MonoBehaviour {

    private GameController gameController;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        transform.LookAt(Vector3.zero);
    }
	
	internal void IncreaseFireRate()
    {
        gameController.firePause *= 0.9f;
    }
}
