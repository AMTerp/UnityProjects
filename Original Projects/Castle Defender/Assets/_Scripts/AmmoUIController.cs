using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIController : MonoBehaviour {

    private Text ammoText;

    // Need to get reference to text component of the UI before the text is set in the 'Start' function 
    // of PlayerShooting.
    void Awake()
    {
        ammoText = GetComponent<Text>();
    }
	
	public void setAmmoCount(int ammoInClip, int spareAmmo)
    {
        ammoText.text = string.Format("{0,3} / {1,3}", ammoInClip, spareAmmo);
    }
}
