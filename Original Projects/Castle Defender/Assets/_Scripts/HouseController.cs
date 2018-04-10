using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {

    public float health;
    public GameObject houseHealthBar;

    private HouseHealth houseHealth;
    internal bool isAlive;

	// Use this for initialization
	void Start () {
        houseHealth = houseHealthBar.GetComponent<HouseHealth>();

        isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void takeDamage(float damage)
    {
        health -= damage;
        houseHealth.updateHealthBar();
    }
}
