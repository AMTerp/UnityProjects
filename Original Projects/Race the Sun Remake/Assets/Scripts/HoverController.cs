using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour {

    public float hoverSpeed;
    public float hoverAmount;
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + Vector3.up * hoverAmount * Mathf.Sin(Time.time * hoverSpeed);
    }
}
