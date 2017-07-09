using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHealth : MonoBehaviour {

    public GameObject house;

    private GameObject healthBar;
    private HouseController houseController;
    private float initHealthHealth;
    private float currHealthHealth;

    // Use this for initialization
    void Start () {
        healthBar = transform.GetChild(2).gameObject;
        houseController = house.GetComponent<HouseController>();
        initHealthHealth = houseController.health;
        currHealthHealth = initHealthHealth;
    }

    public void updateHealthBar()
    {
        currHealthHealth = Mathf.Clamp(houseController.health, 0, initHealthHealth);
        healthBar.transform.localScale = new Vector3(currHealthHealth / initHealthHealth,
            healthBar.transform.localScale.y,
            healthBar.transform.localScale.z);
    }
}
