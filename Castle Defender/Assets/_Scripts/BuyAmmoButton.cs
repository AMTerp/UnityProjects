using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyAmmoButton : MonoBehaviour {

    public int ammoAmount;
    public int ammoCost;
    public string gunName;

    private Button button;
    private Text buttonText;
    private AmmoUIController ammoUI;
    private MoneyController moneyController;
    private GameController gameController;

    // Use this for initialization
    void Start () {
        button = gameObject.GetComponent<Button>();
        buttonText = transform.GetChild(0).gameObject.GetComponent<Text>();
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();
        moneyController = GameObject.FindWithTag("Money UI").GetComponent<MoneyController>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        buttonText.text = string.Format("1x {0} Magazine ({1})", gunName, ammoCost);

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
                
                // Only buy ammo if the player has enough ammo.
                if (gameController.money >= ammoCost)
                {
                    GunController gunController = gunSlot.GetChild(0).gameObject.GetComponent<GunController>();
                    int ammoBefore = gunController.currSpareAmmo;
                    gunController.currSpareAmmo = Mathf.Clamp(gunController.currSpareAmmo + ammoAmount, 0, gunController.maxSpareAmmo);

                    // If the player is currently holding the gun, update the ammo count on the UI.
                    if (gunSlot.GetChild(0).GetChild(0).gameObject.activeSelf)
                    {
                        ammoUI.setAmmoCount(gunController.currAmmoInClip, gunController.currSpareAmmo);
                    }

                    // Update the player's money and update the UI.
                    // Only charge for the ammo that the player actually gains.
                    moneyController.changeMoneyText(-ammoCost * (gunController.currSpareAmmo - ammoBefore) / ammoAmount);
                }
            }
        }
    }
}
