using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text restartText;
    public Text scoreText;
    public int powerupInterval;
    public float spawnWaitMin;
    public float spawnWaitMax;
    public float boundaryThickness;
    public GameObject[] hazards;
    public GameObject[] powerups;
    public Boundary boundary;

    public float zombieSpeed;
    public float zombieSpeedDiff;

    private int score;
    private bool gameOver;
    private bool spawnBoost;
    private int gunSetupCounter = 1;
    private int numGunSetups = 6;
    private float seconds;
    private float zombieInitSpeed;

	// Use this for initialization
	void Start () {
        gameOver = false;
        spawnBoost = false;
        zombieInitSpeed = zombieSpeed;
        restartText.text = "";
        scoreText.text = "Zombies Killed: 0";
        StartCoroutine(SpawnWaves());
        StartCoroutine(DifficultyAdjuster());
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
	}

    IEnumerator DifficultyAdjuster()
    {
        while (!gameOver)
        {
            seconds = Time.timeSinceLevelLoad;
            // This function increases speed by 700 over 2 minutes, given that zombieSpeedDiff = 1.
            zombieSpeed = 70 / 12 * zombieSpeedDiff * seconds + zombieInitSpeed;
            Debug.Log("Speed changed to " + zombieSpeed);
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator SpawnWaves()
    {
        while (!gameOver)
        {
            Vector3 spawnPosition;
            GameObject toSpawn;
            Quaternion spawnRotation = Quaternion.identity;

            if (spawnBoost && gunSetupCounter < numGunSetups)
            {
                spawnPosition = GetPowerupSpawnPos();
                toSpawn = powerups[Random.Range(0, powerups.Length)];
                GameObject clone = Instantiate(toSpawn, spawnPosition, spawnRotation) as GameObject;

                PowerupController powerup = clone.GetComponent<PowerupController>();
                powerup.gunType = gunSetupCounter++;
                spawnBoost = false;
            } else
            {
                spawnPosition = GetHazardSpawnPos();
                toSpawn = hazards[Random.Range(0, hazards.Length)];
                Instantiate(toSpawn, spawnPosition, spawnRotation);
            }

            yield return new WaitForSeconds(Random.Range(spawnWaitMin, spawnWaitMax));
        }
    }

    Vector3 GetHazardSpawnPos()
    {
        float x = Random.Range(boundary.xMax, boundary.xMax + boundaryThickness);
        x = (Random.value > 0.5) ? x : -x;

        float z = Random.Range(0.0f, boundary.zMax + boundaryThickness);
        z = (Random.value > 0.5) ? z : -z;

        if (Random.value > 0.5)
        {
            float temp = x;
            x = z;
            z = temp;
        }

        return new Vector3(x, 0.0f, z);
    }

    Vector3 GetPowerupSpawnPos()
    {
        float x = Random.Range(boundary.xMax, boundary.xMax + boundaryThickness);
        x = (Random.value > 0.5) ? x : -x;

        float z = Random.Range(boundary.zMax, boundary.zMax + boundaryThickness);
        z = (Random.value > 0.5) ? z : -z;

        return new Vector3(x, 0.0f, z);
    }

    internal void BroadcastGameOver()
    {
        GameObject[] sendObjects;
        string[] tags = new string[] { "GameController", "Player", "Enemy" };
        foreach (string tag in tags)
        {
            sendObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject sendObject in sendObjects)
            {
                sendObject.SendMessage("GameOver");
            }
        }
    }

    internal void AddScore()
    {
        score++;
        scoreText.text = "Zombies Killed: " + score;

        if (score % powerupInterval == 0)
        {
            spawnBoost = true;
            powerupInterval *= 2;
        }
    }

    void GameOver()
    {
        gameOver = true;
        restartText.text = "Press 'R' to restart"; 
    }
}
