using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILookX : MonoBehaviour {

    // Makes the game object rotate on the y-axis, facing the target.
    public void LookAtTarget(Transform target)
    {
        transform.LookAt(target.position);
        transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
    }
}
