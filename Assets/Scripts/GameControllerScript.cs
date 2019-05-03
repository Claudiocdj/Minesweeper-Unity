using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public int numBombs;
    public int gridSize;
    public Sprite[] spritesObj;
    public GameObject blockPrefab;

    private int[,] grid;
    public int bombCount = 0;
    public int flags = 0;

    public int timeCount = 0;
    private float time = 0f;

    void Start() {

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
        grid = new int[gridSize, gridSize];

        int x, y, z = 0;

        while (z < numBombs) {
            x = Random.Range(0, gridSize);

            y = Random.Range(0, gridSize);

            if (grid[x, y] != 0) continue;

            grid[x, y] = 9;

            z++;
        }

        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                int n = 0, k = gridSize;

                if (grid[i, j] == 9) continue;

                if (i + 1 < k && grid[i + 1, j] == 9) n++;
                if (j + 1 < k && grid[i, j + 1] == 9) n++;
                if (i - 1 >= 0 && grid[i - 1, j] == 9) n++;
                if (j - 1 >= 0 && grid[i, j - 1] == 9) n++;
                if (i + 1 < k && j + 1 < k && grid[i + 1, j + 1] == 9) n++;
                if (i - 1 >= 0 && j - 1 >= 0 && grid[i - 1, j - 1] == 9) n++;
                if (i - 1 >= 0 && j + 1 < k && grid[i - 1, j + 1] == 9) n++;
                if (i + 1 < k && j - 1 >= 0 && grid[i + 1, j - 1] == 9) n++;

                grid[i, j] = n;
            }
        }
    }

    private void CreateMap() {
        for (int i = 0; i < gridSize; i++)
            for (int j = 0; j < gridSize; j++) {
                GameObject tile = Instantiate(blockPrefab, new Vector3(i, j, 0f), Quaternion.identity);

                tile.transform.parent = gameObject.transform;

                tile.SendMessage("SetOcultSprite", spritesObj[grid[i, j]]);
            }
    }

    public void RemoveFlag(Vector3 pos) {
        flags--;

        if (grid[(int)pos.x, (int)pos.y] == 9)
            bombCount--;
    }

    public void SetFlag(Vector3 pos) {
        flags++;

        if (grid[(int)pos.x, (int)pos.y] == 9)
            bombCount++;

        if (bombCount == numBombs && bombCount == flags) {
            BroadcastMessage("ShowBombBlocks", "win");
            
            StartCoroutine(RestartLevel(numBombs + 1));
        }
    }
    
    public void GameOver() {
        BroadcastMessage("ShowBombBlocks","lose");

        StartCoroutine(RestartLevel(1));
    }

    private IEnumerator RestartLevel(int num) {
        time = -3f;

        yield return new WaitForSeconds(3f);

        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        numBombs = num;
        bombCount = 0;
        flags = 0;

        if (num == 1)
            timeCount = 0;

        LoadMap();
    }
}
