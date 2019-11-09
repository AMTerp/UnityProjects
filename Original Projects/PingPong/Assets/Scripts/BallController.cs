using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float movementSpeed;

    private GameController gameController;
    private Vector2 worldVector;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.roundEndEvent += reset;
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    public void OnCollisionEnter2D(Collision2D aCollision) {
        if (aCollision.contactCount == 0) {
            return;
        }

        worldVector = Vector2.Reflect(worldVector, aCollision.GetContact(0).normal);
    }

    private void move() {
        transform.Translate(worldVector * movementSpeed  * Time.deltaTime, Space.World);
    }

    private void reset() {
        transform.position = Vector2.zero;
        worldVector = transform.up.normalized; // TODO: Randomize, add pause
    }
}
