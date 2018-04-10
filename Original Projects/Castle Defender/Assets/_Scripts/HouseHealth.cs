using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHealth : MonoBehaviour {

    public GameObject house;

    private GameObject healthBar;
    private HouseController houseController;
    private float initHealth;
    private float currHealth;

    // Use this for initialization
    void Start () {
        healthBar = transform.GetChild(2).gameObject;
        houseController = house.GetComponent<HouseController>();
        initHealth = houseController.health;
        currHealth = initHealth;
    }

    public void updateHealthBar()
    {
        currHealth = Mathf.Clamp(houseController.health, 0, initHealth);
        healthBar.transform.localScale = new Vector3(currHealth / initHealth,
            healthBar.transform.localScale.y,
            healthBar.transform.localScale.z);
    }
}
