using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILookY : MonoBehaviour {

    public float lookSpeed;

    private Transform aiCamera;

    void Start ()
    {
        aiCamera = transform.Find("AI Camera");
    }

    // Makes the game object's camera look at the y-coordinate of a given target.
    public void LookAtTarget(Transform target)
    {
        // Kept here for easy switching between Slerp and immediate lookAt.
        //aiCamera.transform.LookAt(target.position);
        //aiCamera.transform.localEulerAngles = new Vector3(aiCamera.transform.localEulerAngles.x, 0.0f, 0.0f);

        Vector3 lookPos = target.position - aiCamera.transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        aiCamera.transform.rotation = Quaternion.Slerp(aiCamera.transform.rotation, rotation, Time.deltaTime * lookSpeed);
        aiCamera.transform.localEulerAngles = new Vector3(aiCamera.transform.localEulerAngles.x, 0.0f, 0.0f);
    }
}
