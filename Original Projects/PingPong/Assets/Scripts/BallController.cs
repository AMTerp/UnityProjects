using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float movementSpeed;
    
    private Vector2 worldVector;

    void Start()
    {
        worldVector = transform.up.normalized;
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
}
