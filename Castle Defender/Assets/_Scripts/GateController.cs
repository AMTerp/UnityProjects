using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

    public float health;
    public GameObject gateHealthParent;

    private GateHealth gateHealth;
    private GameController gameController;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        gateHealth = gateHealthParent.GetComponent<GateHealth>();
	}
	
	public void takeDamage(float damage)
    {
        health -= damage;
        gateHealth.updateHealthBar();
    }
}
