using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public float zSpawnValue;
    public float xSpawnWidth;
    public float initEnemyWaveHp;
    public float enemyWaveHpIncrease;
    public float waveRewardPercent;
    public float waveRewardGoldPerHP;
    public float waveLength;
    public float waveIntermission;
    public GameObject[] hazards;

    internal int money;
    internal bool uiDisableMouseLook;
    internal bool uiDisableMouseClick;

    private bool waveInProgress;
    private float currEnemyWaveHp;
    private int waveNum;
    private WaveCounterUI waveCounterUI;
    private MoneyController moneyController;

    // Use this for initialization
    void Start () {
        // Lock the cursor to the center of the screen and hide it.
        Cursor.lockState = CursorLockMode.Locked;

        waveCounterUI = GameObject.FindWithTag("Wave Counter UI").GetComponent<WaveCounterUI>();
        moneyController = GameObject.FindWithTag("Money UI").GetComponent<MoneyController>();

        currEnemyWaveHp = initEnemyWaveHp;

        money = 800;
        moneyController.setMoneyText(money);
        waveNum = 1;
        waveCounterUI.setWaveCounter(waveNum);

        waveInProgress = true;
        uiDisableMouseLook = false;
        uiDisableMouseLook = false;

        StartCoroutine(SpawnWave());
        StartCoroutine(WaveController());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}

    IEnumerator WaveController()
    {
        while (true)
        {
            if (waveInProgress || !AllEnemiesDead())
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                moneyController.changeMoneyText(GetWaveReward());
                yield return new WaitForSeconds(waveIntermission);
                waveCounterUI.setWaveCounter(++waveNum);
                waveInProgress = true;
                StartCoroutine(SpawnWave());
            }
        }
    }

    IEnumerator SpawnWave()
    {
        float remainingEnemyWaveHp = currEnemyWaveHp;
        float spawnWait = waveLength / (currEnemyWaveHp / 100);

        Vector3 spawnPosition;
        GameObject toSpawn;
        Quaternion spawnRotation = Quaternion.identity;

        while (remainingEnemyWaveHp >= 100)
        {
            toSpawn = hazards[0];
            spawnPosition = GenerateEnemySpawnPos(toSpawn.transform.localScale.y);
            Instantiate(toSpawn, spawnPosition, spawnRotation);

            remainingEnemyWaveHp -= 100;

            yield return new WaitForSeconds(spawnWait);
        }

        currEnemyWaveHp *= enemyWaveHpIncrease;
        Debug.Log("Wave complete! New currEnemyWaveHp: " + currEnemyWaveHp);
        waveInProgress = false;
    }

    Vector3 GenerateEnemySpawnPos(float y)
    {
        return new Vector3(Random.Range(-xSpawnWidth / 2, xSpawnWidth / 2), y, zSpawnValue);
    }

    bool AllEnemiesDead()
    {
        return !((bool) GameObject.FindWithTag("Enemy"));
    }

    int GetWaveReward()
    {
        return (int) (initEnemyWaveHp * Mathf.Pow(enemyWaveHpIncrease, (waveNum - 1)) * waveRewardGoldPerHP * waveRewardPercent);
    }
}
