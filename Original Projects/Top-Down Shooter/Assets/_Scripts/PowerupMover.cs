using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMover : MonoBehaviour {

    public float amplitute;
    public float forwardSpeed;
    public float frequency;

    private Rigidbody rb;
    private GameController gameController;

	// Use this for initialization
	void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameController.gameOver)
        {
            rb.velocity = transform.forward * forwardSpeed + transform.right * Mathf.Sin(Time.time + frequency) * amplitute;
        }
    }
}
