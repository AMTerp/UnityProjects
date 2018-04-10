using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour {

    private Rigidbody rb;
    private GameController gameController;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
	}

    void Update()
    {
        rb.velocity = new Vector3(0.0f, 0.0f, -gameController.DOWN_SPEED);
    }

    void onCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("Collison between hazards");
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
