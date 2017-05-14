using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    private GameController gameController;
    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Enemy"))
            {
                gameController.BroadcastGameOver();
                Debug.Log("Broadcasted");
            } else if (gameObject.CompareTag("Powerup"))
            {
                PowerupController powerup = gameObject.GetComponent<PowerupController>();
                playerController.SetGuns(powerup.numGuns, powerup.angle);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (!(gameObject.CompareTag("Enemy") || gameObject.CompareTag("Powerup")))
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
                gameController.AddScore();
            }
        }
    }
}
