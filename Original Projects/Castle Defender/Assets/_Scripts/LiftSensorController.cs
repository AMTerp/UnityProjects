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
        if (other.gameObject.CompareTag("Player"))
        {
            parentLift.lift = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            parentLift.lift = false;
        }
    }
}
