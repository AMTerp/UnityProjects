using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    internal int score;

    public Text restartText;
    public Text gameOverText;

    private bool gameOver;

    void Start()
    {
        gameOver = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        SetScoreText();
        StartCoroutine (SpawnWaves());
    }

    void Update()
    {
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
        yield return new WaitForSeconds(startWait);
        while(!gameOver)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        SetScoreText();
    }

    void SetScoreText()
    {
        // Padding for the sake of learning. Does not actually look nice in-game because the font (Arial)
        // does not have characters with equal sizes.
        scoreText.text = "Score: " + score.ToString().PadLeft(3, ' ');
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
        restartText.text = "Press 'R' for Restart";
    }
}
