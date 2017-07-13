using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public IEnumerator RunTimer(float time)
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
