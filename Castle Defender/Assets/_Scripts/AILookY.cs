using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILookY : MonoBehaviour {

    private Transform aiCamera;

    void Start ()
    {
        aiCamera = transform.Find("AI Camera");
    }

    // Makes the game object's camera look at the y-coordinate of a given target.
    public void LookAtTarget(Transform target)
    {
        aiCamera.transform.LookAt(target.position);
        aiCamera.transform.localEulerAngles = new Vector3(aiCamera.transform.localEulerAngles.x, 0.0f, 0.0f);
    }
}
