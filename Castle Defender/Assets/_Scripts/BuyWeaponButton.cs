using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWeaponButton : MonoBehaviour {

    public int weaponSlot;
    public int weaponCost;
    public GameObject weapon;
    public AudioClip buySound;
    public float buySoundVolume;

    private Button button;
    private Text buttonText;
    private Vector3 shopPosition;
    private GameController gameController;
    private MoneyController moneyController;

    // Use this for initialization
    void Start () {
        button = gameObject.GetComponent<Button>();
        buttonText = transform.GetChild(0).gameObject.GetComponent<Text>();
        moneyController = GameObject.FindWithTag("Money UI").GetComponent<MoneyController>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        shopPosition = GameObject.FindWithTag("Shop").transform.position;

        buttonText.text = string.Format("Buy {0} ({1})", weapon.name, weaponCost);

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

        // Only buy weapon if the player has enough money.
        if (gameController.money >= weaponCost)
        {
            GameObject newWeapon = Instantiate(weapon, mainCamera.Find("Weapon Slot " + weaponSlot).transform);
            newWeapon.transform.GetChild(0).gameObject.SetActive(false);
            newWeapon.name = weapon.name;

            // Update the player's money and update the UI.
            moneyController.changeMoneyText(-weaponCost);
            AudioSource.PlayClipAtPoint(buySound, shopPosition, buySoundVolume);

            // Play the buy sound and make it non-3D.
            AudioSource buySoundSource = gameController.PlayClipAt(buySound, shopPosition, buySoundVolume);
            buySoundSource.spatialBlend = 0.0f;
        }
    }
}
