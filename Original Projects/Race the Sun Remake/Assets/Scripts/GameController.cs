using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public Text restartText;
    public Text scoreText;
    public float DOWN_SPEED;
    public float startWait;
    public float waveWait;
    public float spawnWaitMin;
    public float spawnWaitMax;
    public float laneWidth;
    public float spawnValueZ;
    public float waveLength;
    public float boostChance;
    public float scoreMultiplier;
    public float difficultyMultiplier;
    public float fadeOutSpeed;
    public Vector2 SIZE;
    public GameObject[] hazards;
    public GameObject[] boosts;

    internal bool gameOver;

    private int score;
    private float difficulty;
    private Vector3 scale;
    private AudioSource music;

    void Start()
    {
        gameOver = false;
        restartText.text = "";
        score = 0;
        UpdateScoreText();

        music = GetComponent<AudioSource>();
        music.Play();

        difficulty = 1;
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        } else
        {
            score += (int) (Time.deltaTime * DOWN_SPEED * scoreMultiplier);
            UpdateScoreText();
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        float timeEnd;
        while (!gameOver)
        {
            timeEnd = Time.time + waveLength;
            while (Time.time < timeEnd)
            {
                if (Random.value < boostChance)
                {
                    GameObject boost = boosts[Random.Range(0, boosts.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(-laneWidth / 2, laneWidth / 2), boost.transform.localScale.y, spawnValueZ);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(boost, spawnPosition, spawnRotation);
                    Instantiate(boost, new Vector3(spawnPosition.x - laneWidth, spawnPosition.y, spawnPosition.z), spawnRotation);
                    Instantiate(boost, new Vector3(spawnPosition.x + laneWidth, spawnPosition.y, spawnPosition.z), spawnRotation);
                }
                else
                {
                    scale.x = Random.Range(SIZE.x, SIZE.y);
                    scale.y = Random.Range(SIZE.x, SIZE.y);
                    scale.z = Random.Range(SIZE.x, SIZE.y);

                    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(-laneWidth / 2, laneWidth / 2), scale.y / 2, spawnValueZ);
                    Quaternion spawnRotation = Quaternion.identity;

                    GameObject clone1 = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                    GameObject clone2 = Instantiate(hazard, new Vector3(spawnPosition.x - laneWidth, spawnPosition.y, spawnPosition.z),
                        spawnRotation) as GameObject;
                    clone2.transform.localScale = clone1.transform.localScale;
                    GameObject clone3 = Instantiate(hazard, new Vector3(spawnPosition.x + laneWidth, spawnPosition.y, spawnPosition.z),
                        spawnRotation) as GameObject;
                    clone3.transform.localScale = clone1.transform.localScale;

                    clone1.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    clone2.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    clone3.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
                }
                yield return new WaitForSeconds(Random.Range(spawnWaitMin, spawnWaitMax * difficulty));
            }
            difficulty /= difficultyMultiplier;
            yield return new WaitForSeconds(waveWait);
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void GameOver()
    {
        gameOver = true;
        DOWN_SPEED = 0;
        StartCoroutine(fadeOut());
        restartText.text = "Press 'R' to restart!";
    }

    IEnumerator fadeOut()
    {
        while (music.volume > 0)
        {
            music.volume -= Time.deltaTime * fadeOutSpeed;
            yield return new WaitForSeconds(0.01f);
        }
        music.Stop();
    }
}
