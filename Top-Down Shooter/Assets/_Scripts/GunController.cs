using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public float firePause;
    public GameObject particle;

    private float nextFire;
    private GameController gameController;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        nextFire = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.parent.CompareTag("Player") && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            Instantiate(particle, transform.position, transform.rotation);
            nextFire = Time.time + firePause;
        }
    }
}
