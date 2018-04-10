using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text scoreText;
    public Text timerText;
    public Text pauseText;
    public Text restartText;
    public RectTransform pauseBG;
    public int levelUpGunsInterval;
    public float boundaryThickness;
    public float firePause;
    public GameObject[] hazards;
    public GameObject gunLevelUp;
    public GameObject fireRateBoost;
    public float fireRateBoostInterval;
    public Boundary boundary;

    public float zombieSpeed;
    public float zombieSpeedDiff;
    public float zombieSpawnDivisor;

    public float spawnWaitMax;
    public float spawnWaitMin;

    public float fadeOutSpeed;

    internal int paused;
    internal bool gameOver;

    private AudioSource music;
    private static int difficulty = 2;
    private int score;
    private bool spawnGunLevelUp;
    private int gunSetupCounter = 1;
    private int numGunSetups = 8;
    private float seconds;
    private float initSpawnMax;
    private float initSpawnMin;
    private float initZombieSpeed;

    // Use this for initialization
    void Start () {
        gameOver = false;
        spawnGunLevelUp = false;
        paused = 0;
        pauseBG.gameObject.SetActive(paused == 1);

        music = GetComponent<AudioSource>();
        music.Play();

        if (difficulty == 1)
        {
            zombieSpeed *= 0.5f;
            spawnWaitMax *= 1.5f;
            spawnWaitMin *= 1.5f;
            zombieSpeedDiff *= 0.8f;
            zombieSpawnDivisor *= 1.2f;
        } else if (difficulty == 3)
        {
            zombieSpeed *= 1.5f;
            spawnWaitMax *= 0.8f;
            spawnWaitMin *= 0.8f;
            zombieSpeedDiff *= 1.2f;
            zombieSpawnDivisor *= 0.8f;
        }

        initZombieSpeed = zombieSpeed;
        initSpawnMax = spawnWaitMax;
        initSpawnMin = spawnWaitMin;

        scoreText.text = "Zombies Killed: 0";
        timerText.text = "Time:   0";
        restartText.text = "";
        SetPauseText();

        StartCoroutine(SpawnWaves());
        StartCoroutine(FireBoostSpawner());
        StartCoroutine(DifficultyAdjuster());
	}
	
	// Update is called once per frame
	void Update () {
		if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 1;
                difficulty = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 1;
                difficulty = 2;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 1;
                difficulty = 3;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Application.Quit();
            }

        }
        else
        {
            // Not gameOver.
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Time.timeScale = paused;
                paused = (paused + 1) % 2;
                SetPauseText();
                pauseBG.gameObject.SetActive(paused == 1);

                if (paused == 1)
                {
                    music.Pause();
                } else
                {
                    music.UnPause();
                }
            }
        }
	}

    IEnumerator FireBoostSpawner()
    {
        Vector3 spawnPosition;
        Quaternion spawnRotation = Quaternion.identity;

        while (!gameOver)
        {
            yield return new WaitForSeconds(fireRateBoostInterval);

            spawnPosition = GetPowerupSpawnPos();
            Instantiate(fireRateBoost, spawnPosition, spawnRotation);
        }
    }

    IEnumerator DifficultyAdjuster()
    {
        while (!gameOver)
        {
            seconds = Time.timeSinceLevelLoad;

            // This function increases speed by 700 over 2 minutes, given that zombieSpeedDiff = 1.
            zombieSpeed = 70.0f / 12 * zombieSpeedDiff * seconds + initZombieSpeed;

            // With this function, spawnWaitMax = 0.2, spawnWaitMin = 0.05 | 
            // seconds = 120, zombieSpawnDivisor = 8, initSpawnMax = 0.5, initSpawnMin = 0.2
            spawnWaitMax = -seconds / (zombieSpawnDivisor * 50) + initSpawnMax;
            spawnWaitMin = -seconds / (zombieSpawnDivisor * 100) + initSpawnMin;

            timerText.text = "Time: " + ((int) seconds).ToString().PadLeft(3, ' ');
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

            if (spawnGunLevelUp && gunSetupCounter < numGunSetups)
            {
                spawnPosition = GetPowerupSpawnPos();
                toSpawn = gunLevelUp;
                GameObject clone = Instantiate(toSpawn, spawnPosition, spawnRotation) as GameObject;

                GunLevelUpController powerup = clone.GetComponent<GunLevelUpController>();
                powerup.gunType = gunSetupCounter++;
                spawnGunLevelUp = false;
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
        string[] tags = new string[] { "GameController", "Player", "Enemy", "GunLevelUp" };
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

        if (score % levelUpGunsInterval == 0)
        {
            spawnGunLevelUp = true;
            levelUpGunsInterval *= 2;
        }
    }

    void SetPauseText()
    {
        if (paused == 1)
        {
            pauseText.text = "Paused";
        } else
        {
            pauseText.text = "";
        }
    }

    void GameOver()
    {
        gameOver = true;
        StartCoroutine(fadeOut());
        Time.timeScale = 0;
        restartText.text = @"
            You died!
            To restart, press:
            '1' for easy
            '2' for medium
            '3' for hard
            or 'E' to exit"; 
    }

    IEnumerator fadeOut()
    {
        while (music.volume > 0)
        {
            music.volume -= fadeOutSpeed;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        music.Stop();
    }
}
