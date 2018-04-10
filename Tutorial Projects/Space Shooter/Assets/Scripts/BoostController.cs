using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour
{
    public int scoreValue;
    public float boost_multiplier;

    private PlayerController playerController;
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

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }

        if (playerController == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.speed_multiplier *= boost_multiplier;
            playerController.fireRate /= boost_multiplier;
            gameController.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
