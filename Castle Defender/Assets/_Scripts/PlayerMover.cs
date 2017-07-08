using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {

    public float movementSpeed;
    public float sprintMultiplier;

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
        movement = movement.normalized * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log(string.Format("Sprint before: ({0}, {1}, {2})", movement.x, movement.y, movement.z));
            movement *= sprintMultiplier;
            Debug.Log(string.Format("Sprint after : ({0}, {1}, {2})", movement.x, movement.y, movement.z));
        }

        rb.velocity = movement;
    }
}
