using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {


    private float nextFire;
    private int currWeaponSlotHeld;
    private GameObject mainCamera;
    private GunController gunController;
    private AmmoUIController ammoUI;

    // Use this for initialization
    void Start () {
        mainCamera = transform.Find("Main Camera").gameObject;
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();

        currWeaponSlotHeld = 1;
        SwapWeapons(1);
    }
	
	// Update is called once per frame
	void Update () {
        CheckControls();
	}

    void CheckControls()
    {
        if (gunController.automatic && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            Fire();
        }

        if (!gunController.automatic && Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && gunController.currAmmoInClip < gunController.magazineSize &&
            gunController.currSpareAmmo > 0 && gunController.canFire)
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapWeapons(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapons(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwapWeapons(3);
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

    void SwapWeapons(int weaponSlot)
    {
        if (!mainCamera.transform.Find("Weapon Slot " + weaponSlot).GetChild(0))
        {
            // Must not have a gun in the specified weapon slot. Don't swap.
            return;
        }

        // Set current held weapon model to inactive.
        mainCamera.transform.Find("Weapon Slot " + currWeaponSlotHeld).GetChild(0).GetChild(0).gameObject.SetActive(false);

        Transform newWeapon = mainCamera.transform.Find("Weapon Slot " + weaponSlot).GetChild(0);

        // Set the new weapon model to active.
        newWeapon.GetChild(0).gameObject.SetActive(true);

        // Get reference to gunController for new weapon.
        gunController = newWeapon.GetComponent<GunController>();
        currWeaponSlotHeld = weaponSlot;

        // Update Ammo UI.
        ammoUI.setAmmoCount(gunController.currAmmoInClip, gunController.currSpareAmmo); // AmmoUI init bug here.
    }
}
