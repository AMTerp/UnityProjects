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

    private int score;
    private bool gameOver;
    private bool spawnBoost;

	// Use this for initialization
	void Start () {
        gameOver = false;
        spawnBoost = false;
        restartText.text = "";
        scoreText.text = "Zombies Killed: 0";
        StartCoroutine(SpawnWaves());
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

    IEnumerator SpawnWaves()
    {
        while (!gameOver)
        {
            Vector3 spawnPosition;
            GameObject toSpawn;
            if (spawnBoost)
            {
                spawnPosition = GetPowerupSpawnPos();
                toSpawn = powerups[Random.Range(0, powerups.Length)];
                spawnBoost = false;
            } else
            {
                spawnPosition = GetHazardSpawnPos();
                toSpawn = hazards[Random.Range(0, hazards.Length)];
            }

            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(toSpawn, spawnPosition, spawnRotation);
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
        }
    }

    void GameOver()
    {
        gameOver = true;
        restartText.text = "Press 'R' to restart"; 
    }
}
