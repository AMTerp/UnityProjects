using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public float firePause;

    private int i;
    private float nextFire;
    private GunController gunController;
    private GameObject mainCamera;

	// Use this for initialization
	void Start () {
        gunController = GetHeldGun();

        nextFire = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        Fire();
	}

    GunController GetHeldGun()
    {
        // Get gun currently being held.
        for (i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.CompareTag("MainCamera"))
            {
                mainCamera = transform.GetChild(i).gameObject;
            }
        }

        for (i = 0; i < mainCamera.transform.childCount; i++)
        {
            if (mainCamera.transform.GetChild(i).gameObject.CompareTag("Gun"))
            {
                Debug.Log("Gun found");
                return mainCamera.transform.GetChild(i).gameObject.GetComponent<GunController>();
            }
        }

        Debug.Log("Could not find gunController");
        return null;
    }

    void Fire()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            gunController.Fire();
            nextFire = Time.time + firePause;
        }
    }
}
