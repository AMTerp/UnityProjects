using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWeaponButton : MonoBehaviour {

    public float cost;
    public int weaponSlot;
    public GameObject weapon;

    private Button button;

    // Use this for initialization
    void Start () {
        button = gameObject.GetComponent<Button>();

        button.onClick.AddListener(buyWeapon);
    }
	
	void buyWeapon()
    {
        // Check that player does not already have the weapon.
        Transform mainCamera = GameObject.FindWithTag("Player").transform.Find("Main Camera");
        Transform weaponSlotObject;
        for (int i = 0; i < mainCamera.childCount; i++)
        {
            weaponSlotObject = mainCamera.GetChild(i);
            if (weaponSlotObject.childCount > 0 && weaponSlotObject.GetChild(0).gameObject.name.Equals(weapon.name))
            {
                // Already have weapon. Return.
                return;
            }
        }

        // Got this far, must not already have weapon.
        GameObject newWeapon = Instantiate(weapon, mainCamera.Find("Weapon Slot " + weaponSlot).transform);
        newWeapon.transform.GetChild(0).gameObject.SetActive(false);
        newWeapon.name = weapon.name;
    }
}
