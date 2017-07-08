using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSensorController : MonoBehaviour {

    private LiftController parentLift;

    void Start()
    {
        parentLift = transform.parent.gameObject.GetComponent<LiftController>();
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            Debug.Log("Player entered");
            parentLift.lift = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Body"))
        {
            Debug.Log("Player exitted");
            parentLift.lift = false;
        }
    }
}
