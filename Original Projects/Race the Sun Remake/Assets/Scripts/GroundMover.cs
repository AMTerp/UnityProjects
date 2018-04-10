using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMover : MonoBehaviour {

    public float TILE_SIZE_Z;

    private Vector3 startPos;
    private GameController gameController;

	// Use this for initialization
	void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float newPos = Mathf.Repeat(Time.time * -gameController.DOWN_SPEED, TILE_SIZE_Z);
        transform.position = startPos + Vector3.forward * newPos;
	}
}
