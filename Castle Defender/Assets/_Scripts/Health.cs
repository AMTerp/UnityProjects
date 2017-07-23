using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float health;

    private GameController gameController;
    private EnemyController enemyController;
    private MoneyController moneyController;
    private EnemyHealthBar healthBar;
    private WaveCounterUI waveCounterUI;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        enemyController = GetComponent<EnemyController>();
        moneyController = GameObject.FindWithTag("Money UI").GetComponent<MoneyController>();
        healthBar = transform.GetChild(1).GetChild(0).GetComponent<EnemyHealthBar>();
        waveCounterUI = GameObject.FindWithTag("Wave Counter UI").GetComponent<WaveCounterUI>();

        health = 100;
	}

    public void TakeDamage(float damage)
    {
        health -= damage;
        gameController.remainingCurrEnemyWaveHp -= damage;
        waveCounterUI.setWaveCounter(gameController.waveNum, 
            gameController.remainingCurrEnemyWaveHp,
            gameController.currEnemyWaveHp);
        CheckDead();
        healthBar.updateHealthBar();
    }

    void CheckDead()
    {
        if (health <= 0)
        {
            moneyController.changeMoneyText(enemyController.moneyReward);
            Destroy(gameObject);
        }
    }
}
