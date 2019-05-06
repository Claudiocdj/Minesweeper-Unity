using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public int numBombs;

    public Sprite[] spritesObj;
    public GameObject blockPrefab;
    public Sprite cursor;

    private int length = 22;
    private int width = 15;
    private int[,] grid;
    
    private int bombCount = 0;
    private int flags = 0;
    private int totalBombsFind = 0;

    private int timeCount = 0;
    private float time = 0f;

    private CameraScript cameraScript;
    private GameOverScript goScript;

    void Start() {
        Cursor.SetCursor(cursor.texture, new Vector2(0f,0f), CursorMode.Auto);

        cameraScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();

        goScript = GameObject.FindWithTag("GameOver").GetComponent<GameOverScript>();

        LoadMap();
    }

    private void Update() {
        time += Time.deltaTime;

        if (time >= 1f) {
            timeCount++;
            time = 0;
        }
    }

    private void LoadMap() {
        CreateGrid();

        CreateMap();
    }

    private void CreateGrid() {
        grid = new int[length, width];

        int x, y, z = 0;

        while (z < numBombs) {
            x = Random.Range(0, length);

            y = Random.Range(0, width);

            if (grid[x, y] != 0) continue;

            grid[x, y] = 9;

            z++;
        }

        for (int i = 0; i < length; i++) {
            for (int j = 0; j < width; j++) {
                int n = 0;

                if (grid[i, j] == 9) continue;

                if (i + 1 < length && grid[i + 1, j] == 9) n++;
                if (j + 1 < width && grid[i, j + 1] == 9) n++;
                if (i - 1 >= 0 && grid[i - 1, j] == 9) n++;
                if (j - 1 >= 0 && grid[i, j - 1] == 9) n++;
                if (i + 1 < length && j + 1 < width && grid[i + 1, j + 1] == 9) n++;
                if (i - 1 >= 0 && j - 1 >= 0 && grid[i - 1, j - 1] == 9) n++;
                if (i - 1 >= 0 && j + 1 < width && grid[i - 1, j + 1] == 9) n++;
                if (i + 1 < length && j - 1 >= 0 && grid[i + 1, j - 1] == 9) n++;

                grid[i, j] = n;
            }
        }
    }

    private void CreateMap() {
        for (int i = 0; i < length; i++)
            for (int j = 0; j < width; j++) {
                GameObject tile = Instantiate(blockPrefab, new Vector3(i, j, 0f), Quaternion.identity);

                tile.transform.parent = gameObject.transform;

                tile.SendMessage("SetOcultSprite", spritesObj[grid[i, j]]);
            }
    }

    public void RemoveFlag(Vector3 pos) {
        flags--;

        if (grid[(int)pos.x, (int)pos.y] == 9)
            bombCount--;

        CheckWin();
    }

    public void SetFlag(Vector3 pos) {
        flags++;

        if (grid[(int)pos.x, (int)pos.y] == 9)
            bombCount++;

        CheckWin();
    }

    public void CheckWin() {
        if (bombCount == numBombs && bombCount == flags) {
            BroadcastMessage("ShowBombBlocks", "win");

            totalBombsFind += bombCount;

            StartCoroutine(StartLevel(numBombs + 5));
        }
    }

    private IEnumerator StartLevel(int num) {
        time = -3f;

        yield return new WaitForSeconds(3f);

        goScript.hiddenGameOver();

        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);
        
        numBombs = num;

        bombCount = 0;

        flags = 0;

        LoadMap();
    }

    public void GameOver() {
        BroadcastMessage("ShowBombBlocks", "lose");

        cameraScript.ShakeCamera();

        goScript.OnGameOver(totalBombsFind + bombCount);

        numBombs = 10;
    }

    public void RestartGame() {
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        LoadMap();

        timeCount = totalBombsFind = bombCount = flags = 0;
    }

    public int TimeCount {
        get{ return timeCount; }
    }

    public int Flags {
        get { return flags; }
    }
    
}
