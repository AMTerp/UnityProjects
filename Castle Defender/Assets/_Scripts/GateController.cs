using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

    private GameController gameController;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
