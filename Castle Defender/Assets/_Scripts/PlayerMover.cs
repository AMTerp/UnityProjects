using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

    public float movementSpeed;

    private Rigidbody rb;
    private Vector3 moveX;
    private Vector3 moveY;
    private Vector3 movement;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        moveX = rb.transform.right * Input.GetAxisRaw("Horizontal");
        moveY = rb.transform.forward * Input.GetAxisRaw("Vertical");

        movement = moveX + moveY;

        // Going just straight is slower than going diagonal. Find fix.
        rb.velocity = movement.normalized * movementSpeed * Time.deltaTime;
    }
}
