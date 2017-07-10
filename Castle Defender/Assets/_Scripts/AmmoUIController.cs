using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIController : MonoBehaviour {

    private Text ammoText;

	// Use this for initialization
	void Start () {
        ammoText = GetComponent<Text>();
	}
	
	public void setAmmoCount(int ammoInClip, int spareAmmo)
    {
        ammoText.text = string.Format("{0,3} / {1,3}", ammoInClip, spareAmmo);
    }
}
