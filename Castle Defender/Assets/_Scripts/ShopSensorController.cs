using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSensorController : MonoBehaviour {

    private ShopController parentShop;

	// Use this for initialization
	void Start () {
        parentShop = transform.parent.gameObject.GetComponent<ShopController>();
    }
	
	void OnTriggerEnter(Collider other)
    {
        parentShop.EnableShopScreen();
    }

    void OnTriggerExit(Collider other)
    {
        parentShop.DisableShopScreen();
    }
}
