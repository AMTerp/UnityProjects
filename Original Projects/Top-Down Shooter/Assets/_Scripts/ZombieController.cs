using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {

    private float speed;
    private Rigidbody rb;
    private bool gameOver;
    private GameObject player;
    private GameController gameController;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        rb = GetComponent<Rigidbody>();
        speed = gameController.zombieSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        gameOver = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!gameOver)
        {
            moveToPlayer();
        }
	}

    void moveToPlayer()
    {
        transform.LookAt(player.transform);
        rb.velocity = transform.forward * speed * Time.deltaTime;
    }

    void GameOver()
    {
        gameOver = true;
        rb.velocity = Vector3.zero;
    }
}
