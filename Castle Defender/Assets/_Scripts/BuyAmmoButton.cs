using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyAmmoButton : MonoBehaviour {

    public int ammoAmount;
    public float ammoCost;
    public string gunName;

    private Button button;
    private AmmoUIController ammoUI;

	// Use this for initialization
	void Start () {
        button = gameObject.GetComponent<Button>();
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();

        button.onClick.AddListener(buyAmmo);
    }
	
	void buyAmmo()
    {
        // Check that player has the relevant gun.
        Transform mainCamera = GameObject.FindWithTag("Player").transform.Find("Main Camera");
        Transform gunSlot;
        for (int i = 0; i < mainCamera.childCount; i++)
        {
            gunSlot = mainCamera.GetChild(i);
            if (gunSlot.childCount > 0 && gunSlot.GetChild(0).gameObject.name.Equals(gunName))
            {
                // Player has the relevant gun.
                GunController gunController = gunSlot.GetChild(0).gameObject.GetComponent<GunController>();
                int ammoBefore = gunController.currSpareAmmo;
                gunController.currSpareAmmo = Mathf.Clamp(gunController.currSpareAmmo + ammoAmount, 0, gunController.maxSpareAmmo);
                ammoUI.setAmmoCount(gunController.currAmmoInClip, gunController.currSpareAmmo);
                // Now, using ammoBefore, determine how much to charge the player.
            }
        }
    }
}
