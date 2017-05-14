using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour {

    public float amplitute;
    public float forwardSpeed;
    public float frequency;
    public float gunOffset;
    public GameObject gun;

    internal int numGuns;
    internal float angle;

    private Rigidbody rb;
    private GameObject player;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        numGuns = 4;
        angle = 270;
        SetGuns(4, 270);
        transform.LookAt(Vector3.zero);
    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        Vector3 movement = new Vector3(amplitute * Mathf.Sin(Time.time + frequency), 0.0f, transform.forward.z * forwardSpeed);
        rb.velocity = movement;
    }

    void SetGuns(int num, float totalAngle)
    {
        float angleChange;

        if (num == 1)
        {
            angleChange = 0;
        }
        else
        {
            angleChange = totalAngle / (num - 1);
        }

        float angle = -angleChange;
        float x, z;
        Vector3 spawnPos;
        GameObject clone;
        for (int i = 0; i < num; i++)
        {
            x = gunOffset * Mathf.Sin(Mathf.Deg2Rad * angle);
            z = gunOffset * Mathf.Cos(Mathf.Deg2Rad * angle);
            spawnPos = new Vector3(x, 0.0f, z) + transform.position;
            clone = Instantiate(gun, spawnPos, Quaternion.identity) as GameObject;
            clone.transform.Rotate(new Vector3(0, angle, 0));
            clone.transform.parent = transform;

            angle += angleChange;
        }
    }
}
