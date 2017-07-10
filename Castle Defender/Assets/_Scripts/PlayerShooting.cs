using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {


    private int i;
    private float nextFire;
    private GunController gunController;
    private GameObject mainCamera;

    // Use this for initialization
    void Start () {
        gunController = GetHeldGun();
	}
	
	// Update is called once per frame
	void Update () {
        CheckControls();
	}

    GunController GetHeldGun()
    {
        GunController gun;

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
                gun = mainCamera.transform.GetChild(i).gameObject.GetComponent<GunController>();
                return gun;
            }
        }

        Debug.Log("Could not find gunController");
        return null;
    }

    void CheckControls()
    {
        if (gunController.automatic && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            Fire();
        }
        else if(!gunController.automatic && Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.R) && gunController.currAmmoInClip < gunController.magazineSize &&
            gunController.currSpareAmmo > 0 && gunController.canFire)
        {
            Reload();
        }
    }

    void Fire()
    {
        gunController.Fire();
        nextFire = Time.time + gunController.firePause;
    }

    void Reload()
    {
        StartCoroutine(gunController.Reload());
        nextFire = Time.time + gunController.reloadTime;
    }
}
