using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

    public Canvas gameOver;
    public Text totalTime;
    public Text totalBombsFind;

    private GameControllerScript gcScript;

    void Start() {
        gcScript = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();

        gameOver.enabled = false;
    }

    public void OnGameOver(int totalBombs) {
        totalTime.text = gcScript.TimeCount.ToString();

        totalBombsFind.text = totalBombs.ToString();

        gameOver.enabled = true;
    }

    public void hiddenGameOver() {
        gameOver.enabled = false;
    }

    public void Restart() {
        gameOver.enabled = false;

        gcScript.RestartGame();
    }
}
