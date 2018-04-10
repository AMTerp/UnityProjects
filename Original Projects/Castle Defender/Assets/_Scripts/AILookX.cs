using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILookX : MonoBehaviour {

    public float lookSpeed;

    // Makes the game object rotate on the y-axis, facing the target.
    public void LookAtTarget(Transform target)
    {
        // Kept here for easy switching between Slerp and immediate lookAt.
        //transform.LookAt(target.position);
        //transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);

        // Adapted from code written by Mike 3.
        // Link: http://answers.unity3d.com/answers/36256/view.html
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }
}
