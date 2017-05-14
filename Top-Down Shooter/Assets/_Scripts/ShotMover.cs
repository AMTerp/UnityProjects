using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMover : MonoBehaviour {

    public float shotSpeed;
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * shotSpeed;
	}
}
