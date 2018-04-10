using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMover : Mover {

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

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * (speed - gameController.score / 50.0f);
        //Debug.Log("Spawned asteroid with velocity: " + rb.velocity.magnitude);
    }
}
