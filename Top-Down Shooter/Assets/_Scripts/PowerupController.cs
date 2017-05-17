﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GunSetup {
    internal int NumGuns { get; set; }
    internal float Angle { get; set; }
    public GunSetup(int numGuns, float angle)
    {
        NumGuns = numGuns;
        Angle = angle;
    }
}

public class PowerupController : MonoBehaviour {

    public float amplitute;
    public float forwardSpeed;
    public float frequency;
    public float gunOffset;
    public GameObject gun;

    private Rigidbody rb;
    private GameObject player;
        
    internal int gunType;
    internal int[] numGuns = { 1, 2, 3, 4, 5, 6 };
    internal float[] angles = { 0, 10, 20, 270, 40, 315 };

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetGuns(numGuns[gunType], angles[gunType]);
        transform.LookAt(Vector3.zero);
    }
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        Vector3 movement = new Vector3(amplitute * Mathf.Sin(Time.time + frequency), 0.0f, 0.0f);
        rb.velocity = transform.forward * forwardSpeed + movement;
    }

    // Try to remove this function. Already exists in PlayerController.cs
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