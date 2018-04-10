using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;

    private bool gameOver;
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

        gameOver = false;
    }
	
	void FixedUpdate () {
        if (!gameOver)
        {
            float moveX = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(moveX, 0.0f, 0.0f);

            rb.AddForce(movement * speed);

            rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);

            if (Mathf.Abs(transform.position.x) > gameController.laneWidth / 2)
            {
                transform.position = new Vector3(-transform.position.x + rb.velocity.x * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }
    }

    void GameOver()
    {
        gameOver = true;
    }
}
