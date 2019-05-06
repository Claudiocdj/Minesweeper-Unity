using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    public Text time;
    public Text flags;

    private GameControllerScript gameController;

    void Start() {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
    }

    void Update() {
        time.text = gameController.TimeCount.ToString();
        
        flags.text = (gameController.numBombs - gameController.Flags).ToString();
    }
}
