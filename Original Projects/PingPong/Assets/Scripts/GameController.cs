using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public event Action roundEndEvent;

    private static int topScore = 0;
    private static int bottomScore = 0;

    public BallController ball;
    public PaddleMovement topPaddle;
    public PaddleMovement bottomPaddle;
    public Text topScoreText;
    public GoalController topGoal;
    public Text bottomScoreText;
    public GoalController bottomGoal;

    void Start() {
        topGoal.goalEvent += handleScoreOnTop;
        bottomGoal.goalEvent += handleScoreOnBottom;

        updateScore(topScoreText, 0);
        updateScore(bottomScoreText, 0);
    }

    public void handleScoreOnTop() {
        incrementBottomScore();
        beginNextRound();
    }

    public void handleScoreOnBottom() {
        incrementTopScore();
        beginNextRound();
    }

    private void beginNextRound() {
        roundEndEvent();
    }

    private void incrementTopScore() {
        topScore++;
        updateScore(topScoreText, topScore);
    }

    private void incrementBottomScore() {
        bottomScore++;
        updateScore(bottomScoreText, bottomScore);
    }

    private void updateScore(Text aText, int aScore) {
        aText.text = string.Format("{0:#0}", aScore);
    }
}
