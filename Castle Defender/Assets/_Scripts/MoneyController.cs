using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour {

    private Text moneyText;
    private GameController gameController;

	void Awake() {
        moneyText = GetComponent<Text>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }
	
    public void changeMoneyText(int deltaMoney)
    {
        gameController.money += deltaMoney;
        setMoneyText(gameController.money);
    }

	public void setMoneyText(int money)
    {
        moneyText.text = string.Format("Money: {0, 6}", money);
    }
}
