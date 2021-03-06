﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public int targetFrameRate;
    public bool enableVsync;
    public float zSpawnValue;
    public float xSpawnWidth;
    public float initEnemyWaveHp;
    public float enemyWaveHpIncrease;
    public float waveRewardPercent;
    public float waveRewardGoldPerHP;
    public float waveLength;
    public float waveLengthIncrease;
    public float waveIntermission;
    public GameObject[] hazards;

    internal int money;
    internal int waveNum;
    internal bool uiDisableMouseLook;
    internal bool uiDisableMouseClick;
    internal float currEnemyWaveHp;
    internal float remainingCurrEnemyWaveHp;

    private bool waveInProgress;
    private WaveCounterUI waveCounterUI;
    private MoneyController moneyController;

    void Awake()
    {
        if (!enableVsync)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }
    }

    // Use this for initialization
    void Start () {
        // Lock the cursor to the center of the screen and hide it.
        Cursor.lockState = CursorLockMode.Locked;

        waveCounterUI = GameObject.FindWithTag("Wave Counter UI").GetComponent<WaveCounterUI>();
        moneyController = GameObject.FindWithTag("Money UI").GetComponent<MoneyController>();

        currEnemyWaveHp = initEnemyWaveHp;
        remainingCurrEnemyWaveHp = currEnemyWaveHp;

        money = 800;
        moneyController.setMoneyText(money);
        waveNum = 1;
        waveCounterUI.setWaveCounter(waveNum, currEnemyWaveHp, currEnemyWaveHp);

        waveInProgress = true;
        uiDisableMouseLook = false;
        uiDisableMouseLook = false;

        StartCoroutine(SpawnWave());
        StartCoroutine(WaveController());
	}
	
	// Update is called once per frame
	void Update () {
        CheckInputs();
	}

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            moneyController.changeMoneyText(1000);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (AudioListener.volume > 0.0f)
            {
                AudioListener.volume = 0.0f;
            }
            else
            {
                AudioListener.volume = 1.0f;
            }
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
                waveLength += waveLengthIncrease;
                currEnemyWaveHp *= enemyWaveHpIncrease;
                remainingCurrEnemyWaveHp = currEnemyWaveHp;
                yield return new WaitForSeconds(waveIntermission);
                waveCounterUI.setWaveCounter(++waveNum, remainingCurrEnemyWaveHp, currEnemyWaveHp);
                waveInProgress = true;
                StartCoroutine(SpawnWave());
            }
        }
    }

    IEnumerator SpawnWave()
    {
        float remainingHpToSpawn = currEnemyWaveHp;
        float spawnWait = waveLength / (currEnemyWaveHp / 100);

        Vector3 spawnPosition;
        GameObject toSpawn;
        Quaternion spawnRotation = Quaternion.identity;

        while (remainingHpToSpawn >= 100)
        {
            toSpawn = hazards[0];
            spawnPosition = GenerateEnemySpawnPos(toSpawn.transform.localScale.y);
            Instantiate(toSpawn, spawnPosition, spawnRotation);

            remainingHpToSpawn -= 100;

            yield return new WaitForSeconds(spawnWait);
        }

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

    //// Adapted version of function written by aldonaletto
    //// Link: http://answers.unity3d.com/questions/316575/adjust-properties-of-audiosource-created-with-play.html?childToView=410492#comment-410492
    //// Kept in this code for future reference; not currently used in this project.
    //public AudioSource PlayClipAt(AudioClip clip, Vector3 pos, float volume)
    //{
    //    GameObject tempGO = new GameObject("TempAudio"); // create the temp object
    //    tempGO.transform.position = pos; // set its position
    //    AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
    //    aSource.PlayOneShot(clip, volume); // start the sound at the desired volume
    //    Destroy(tempGO, clip.length); // destroy object after clip duration
    //    return aSource; // return the AudioSource reference
    //}
}
