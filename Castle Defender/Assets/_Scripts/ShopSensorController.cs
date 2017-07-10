using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSensorController : MonoBehaviour {

    private ShopController parentShop;

    private GameController gameController;

	// Use this for initialization
	void Start () {
        parentShop = transform.parent.gameObject.GetComponent<ShopController>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }
	
	void OnTriggerEnter(Collider other)
    {
        parentShop.EnableShopScreen();
        gameController.uiDisableMouseClick = true;
        gameController.uiDisableMouseLook = true;
    }

    void OnTriggerExit(Collider other)
    {
        parentShop.DisableShopScreen();
        gameController.uiDisableMouseClick = false;
        gameController.uiDisableMouseLook = false;
    }
}
