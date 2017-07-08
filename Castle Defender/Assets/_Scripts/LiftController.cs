using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour {

    public float speed;
    public float maxHeight;

    internal bool lift;

    private float minHeight = 0.1f;

	// Use this for initialization
	void Start () {
        lift = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (lift)
        {
            elevate();
        }
        else
        {
            lower();
        }
	}

    // Elevate one step, scales properly with frame rate.
    public void elevate()
    {
        transform.position = new Vector3(transform.position.x, 
            Mathf.Clamp(transform.position.y + speed * Time.deltaTime, minHeight, maxHeight), 
            transform.position.z);
    }

    // Lower one step, scales properly with frame rate.
    public void lower()
    {
        transform.position = new Vector3(transform.position.x,
            Mathf.Clamp(transform.position.y - speed * Time.deltaTime, minHeight, maxHeight),
            transform.position.z);
    }
}
