using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyAmmoButton : MonoBehaviour, IPointerEnterHandler {

    public int ammoAmount;
    public int ammoCost;
    public string gunName;
    public AudioClip buySound;
    public float buySoundVolume;
    public AudioClip denySound;
    public float denyVolume;
    public AudioClip hoverSound;
    public float hoverVolume;
    public GameObject shopAudioGameObject;

    private Button button;
    private Text buttonText;
    private AmmoUIController ammoUI;
    private AudioSource shopAudioSource;
    private MoneyController moneyController;
    private GameController gameController;
    private int ammoBefore;

    // Use this for initialization
    void Start () {
        button = gameObject.GetComponent<Button>();
        buttonText = transform.GetChild(0).gameObject.GetComponent<Text>();
        ammoUI = GameObject.FindWithTag("Ammo UI").GetComponent<AmmoUIController>();
        shopAudioSource = shopAudioGameObject.GetComponent<AudioSource>();
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
        int i;
        for (i = 0; i < mainCamera.childCount; i++)
        {
            gunSlot = mainCamera.GetChild(i);
            if (gunSlot.childCount > 0 && gunSlot.GetChild(0).gameObject.name.Equals(gunName))
            {
                // Player has the relevant gun.

                // Only buy ammo if the player has enough money.
                if (gameController.money >= ammoCost)
                {
                    GunController gunController = gunSlot.GetChild(0).gameObject.GetComponent<GunController>();
                    ammoBefore = gunController.currSpareAmmo;
                    gunController.currSpareAmmo = Mathf.Clamp(gunController.currSpareAmmo + ammoAmount, 
                        0, 
                        gunController.maxSpareAmmo + gunController.magazineSize - gunController.currAmmoInClip);

                    // If the player is currently holding the gun, update the ammo count on the UI.
                    if (gunSlot.GetChild(0).GetChild(0).gameObject.activeSelf)
                    {
                        ammoUI.setAmmoCount(gunController.currAmmoInClip, gunController.currSpareAmmo);
                    }

                    // Update the player's money and update the UI.
                    // Only charge for the ammo that the player actually gains.
                    moneyController.changeMoneyText(-ammoCost * (gunController.currSpareAmmo - ammoBefore) / ammoAmount);

                    // Play the buy sound only if ammo was actually bought.
                    if (ammoBefore < gunController.currSpareAmmo)
                    {
                        shopAudioSource.PlayOneShot(buySound, buySoundVolume);
                    }
                    else
                    {
                        // No ammo was bought.
                        PlayDenySound();
                    }
                }
                else
                {
                    // Player does not have enough money to buy ammo.
                    PlayDenySound();
                }

                // Relevant gun was found. Stop searching.
                break;
            }
        }

        if (i == mainCamera.childCount)
        {
            // Player did not have the relevant gun.
            PlayDenySound();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered");
        shopAudioSource.PlayOneShot(hoverSound, hoverVolume);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exitted");
        shopAudioSource.PlayOneShot(hoverSound, hoverVolume);
    }

    void PlayDenySound()
    {
        shopAudioSource.PlayOneShot(denySound, denyVolume);
    }
}