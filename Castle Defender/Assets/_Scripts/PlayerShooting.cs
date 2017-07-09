using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {


    private int i;
    private float nextFire;
    private float firePause;
    private GunController gunController;
    private GameObject mainCamera;

	// Use this for initialization
	void Start () {
        gunController = GetHeldGun();
        firePause = gunController.firePause;
        nextFire = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        Fire();
	}

    GunController GetHeldGun()
    {
        // Get player camera.
        for (i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.CompareTag("MainCamera"))
            {
                mainCamera = transform.GetChild(i).gameObject;
                break;
            }
        }
        
        // Get player gun.
        for (i = 0; i < mainCamera.transform.childCount; i++)
        {
            if (mainCamera.transform.GetChild(i).gameObject.CompareTag("Gun"))
            {
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
