using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLightController : MonoBehaviour {

    public Light frontLighting;

    private float initAngle;
    private float ratio;
    private float initIntensity;
    private Light thisLight;

	// Called at initialization.
	void Start () {
        initAngle = frontLighting.transform.eulerAngles.x;
        thisLight = GetComponent<Light>();
        initIntensity = thisLight.intensity;
	}
	
	// Called every frame.
	void Update () {

        // Scales the intensity of back light with the angle of the front light (sun).
        // Ensures that the intensity is never scaled above 100% of its initial value.
        ratio = frontLighting.transform.eulerAngles.x / initAngle;
        ratio = ratio > 1 ? 1 : ratio;

        // Ensures that back light stays at 0 as the sun goes below the horizon.
        ratio = frontLighting.transform.eulerAngles.x > 350 ? 0 : ratio;
        thisLight.intensity = ratio * initIntensity;
    }
}
