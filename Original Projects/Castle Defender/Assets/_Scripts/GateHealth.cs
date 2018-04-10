using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateHealth : MonoBehaviour {

    public GameObject gate;

    private GameObject healthBar;
    private GateController gateController;
    private float initGateHealth;
    private float currGateHealth;

    // Use this for initialization
    void Start () {
        healthBar = transform.GetChild(2).gameObject;
        gateController = gate.GetComponent<GateController>();
        initGateHealth = gateController.health;
        currGateHealth = initGateHealth;
	}

    public void updateHealthBar()
    {
        currGateHealth = Mathf.Clamp(gateController.health, 0, initGateHealth);
        healthBar.transform.localScale = new Vector3(currGateHealth / initGateHealth, 
            healthBar.transform.localScale.y, 
            healthBar.transform.localScale.z);
    }
}
