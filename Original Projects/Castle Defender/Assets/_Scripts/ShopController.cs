using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour {

    public GameObject shopScreen;

	// Use this for initialization
	void Start () {
        DisableShopScreen();
	}
	
	public void EnableShopScreen()
    {
        shopScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableShopScreen()
    {
        shopScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
