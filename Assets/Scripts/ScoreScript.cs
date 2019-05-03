using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public Text time;
    public Text level;
    public Text flags;

    private GameControllerScript gameController;

    void Start() {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
    }

    void Update() {
        time.text = gameController.timeCount.ToString();

        level.text = gameController.numBombs.ToString();

        flags.text = gameController.flags.ToString();
    }
}
