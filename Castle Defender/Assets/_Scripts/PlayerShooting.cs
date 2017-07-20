using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    private int currWeaponSlotHeld;
    private GameObject mainCamera;
    private GunController gunController;
    private GameController gameController;
    private AmmoUIController ammoUI;
    private BulletTimer bulletTimerUI;

    // Use this for initialization
    void Start () {
        mainCamera = transform.Find("Main Camera").gameObject;
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        bulletTimerUI = GameObject.FindWithTag("Bullet Timer").GetComponent<BulletTimer>();

        currWeaponSlotHeld = 1;
        gunController = mainCamera.transform.Find("Weapon Slot " + currWeaponSlotHeld).GetChild(0).GetComponent<GunController>();

        ammoUI.setAmmoCount(gunController.currAmmoInClip, gunController.currSpareAmmo);
    }

    // Update is called once per frame
    void Update () {
        CheckControls();
	}

    void CheckControls()
    {
        if (!gameController.uiDisableMouseClick)
        {
            if (gunController.automatic && Input.GetMouseButton(0) && Time.time > gunController.nextFire)
            {
                gunController.Fire();
            }

            if (!gunController.automatic && Input.GetMouseButtonDown(0) && Time.time > gunController.nextFire)
            {
                gunController.Fire();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(gunController.Zoom());
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
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWeapons(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwapWeapons(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwapWeapons(4);
        }
    }

    void Reload()
    {
        StartCoroutine(gunController.Reload());
    }

    void SwapWeapons(int weaponSlot)
    {
        if (!mainCamera.transform.Find("Weapon Slot " + weaponSlot).GetChild(0))
        {
            // Must not have a gun in the specified weapon slot. Don't swap.
            return;
        }

        if (gunController.zoomedIn)
        {
            StartCoroutine(gunController.Zoom());
        }

        // Set current held weapon model to inactive.
        gunController.canFire = true; // Indicates to current gunController to cancel reload.
        gunController.animations.Stop(); // Stop any animations, particularly bullet chambering animations.
        mainCamera.transform.Find("Weapon Slot " + currWeaponSlotHeld).GetChild(0).GetChild(0).gameObject.SetActive(false);

        Transform newWeapon = mainCamera.transform.Find("Weapon Slot " + weaponSlot).GetChild(0);

        // Set the new weapon model to active.
        newWeapon.GetChild(0).gameObject.SetActive(true);

        // Get reference to gunController for new weapon and reset transform.
        gunController = newWeapon.GetComponent<GunController>();
        gunController.resetTransform();
        currWeaponSlotHeld = weaponSlot;

        // Update UI.
        ammoUI.setAmmoCount(gunController.currAmmoInClip, gunController.currSpareAmmo);
        if (gunController.currAmmoInClip > 0)
        {
            StartCoroutine(bulletTimerUI.RunTimer(0.0f, "full"));
        }
        else
        {
            StartCoroutine(bulletTimerUI.RunTimer(0.0f, "empty"));
        }
    }
}
