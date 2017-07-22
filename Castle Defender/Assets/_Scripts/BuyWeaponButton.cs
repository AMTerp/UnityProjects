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
    public GameObject shopAudioGameObject;

    private Button button;
    private Text buttonText;
    private AudioSource shopAudioSource;
    private GameController gameController;
    private MoneyController moneyController;

    // Use this for initialization
    void Start () {
        button = gameObject.GetComponent<Button>();
        buttonText = transform.GetChild(0).gameObject.GetComponent<Text>();
        shopAudioSource = shopAudioGameObject.GetComponent<AudioSource>();
        moneyController = GameObject.FindWithTag("Money UI").GetComponent<MoneyController>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        buttonText.text = string.Format("Buy {0} ({1})", weapon.name, weaponCost);

        button.onClick.AddListener(BuyWeapon);
    }

    void BuyWeapon()
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

            // Play the buy sound.
            shopAudioSource.PlayOneShot(buySound, buySoundVolume);
        }
    }
}
