using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float health;

    private GameController gameController;
    private EnemyController enemyController;
    private MoneyController moneyController;
    private EnemyHealthBar healthBar;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        enemyController = GetComponent<EnemyController>();
        moneyController = GameObject.FindWithTag("Money UI").GetComponent<MoneyController>();
        healthBar = transform.GetChild(0).GetChild(0).GetComponent<EnemyHealthBar>();

        health = 100;
	}

    public void TakeDamage(float damage)
    {
        health -= damage;
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
