using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public IEnumerator RunTimer(float time, string type="normal")
    {
        // type: "normal" for regular behavior that makes use of 'time'. "empty" to make the bar invisible.
        // "full" to instantly refill the bar.

        if (type.Equals("empty"))
        {
            // Just make the bar invisible (likely due to last bullet just having been fired).
            transform.localScale = new Vector3(1, 0, 1);
        }
        else if (type.Equals("full"))
        {
            // Instantly make the bar full (likely due to finish reload).
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            float currTime = 0.0f;
            transform.localScale = new Vector3(1, 0, 1);
            while (transform.localScale.y < 1)
            {
                transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, currTime / (time - 0.001f)), 1);
                yield return new WaitForEndOfFrame();
                currTime += Time.deltaTime;
            }
        }
    }
}
