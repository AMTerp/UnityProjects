using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPaddleController : MonoBehaviour
{
    private const float MIN_BALL_DISTANCE_THRESHOLD = 0.1f;

    public PaddleMovement paddleMovementScript;
    public Transform ball;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move() {
        float xDistanceToBall = ball.position.x - transform.position.x;

        if (Mathf.Abs(xDistanceToBall) < MIN_BALL_DISTANCE_THRESHOLD) {
            return;
        }

        if (xDistanceToBall > 0) {
            paddleMovementScript.right(speed);
        } else {
            paddleMovementScript.left(speed);
        }
    }
}
