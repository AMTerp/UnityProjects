using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddleMovement : MonoBehaviour
{
    public PaddleMovement movementScript;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            movementScript.left(speed);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            movementScript.right(speed); 
        }
    }
}
