using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostContact : MonoBehaviour {

    public float checkRate;
    public float speedUpDuration;
    public float speedUpMultiplier;

    private float revertTime;
    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.position = new Vector3(0.0f, -1000, 0.0f);
            StartCoroutine(SpeedUp());
        }
    }

    IEnumerator SpeedUp()
    {
        float timeSinceBoost;
        float timeBegin = Time.time;
        float originalDownSpeed = gameController.DOWN_SPEED;
        while(Time.time < (timeBegin + speedUpDuration) && !gameController.gameOver)
        {
            timeSinceBoost = Time.time - timeBegin;
            gameController.DOWN_SPEED = -((speedUpMultiplier - 1) * originalDownSpeed) * 
                Mathf.Abs(timeSinceBoost / (speedUpDuration / 2) - 1) + speedUpMultiplier * originalDownSpeed;
            yield return new WaitForSeconds(checkRate);
        }
    }
}
