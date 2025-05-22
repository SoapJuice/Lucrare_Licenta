using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Component player;
    [SerializeField]
    private Tile tile;
    [SerializeField]
    private SlowTile slowTile;
    [SerializeField]
    private WallTile wall;
    [SerializeField]
    private GameObject enemy;

    private Tile endTile;
    private int[,] level;
    private static int playCount = 0; // Track how many times we've played

    private void Start()
    {
        // For first 3 plays, use random map, then switch to adaptive
        if (playCount < 3)
        {
            level = RandomMapGenerator.GenerateLevel(GameManager.Instance.lastLevelResult);
            AdaptiveMapGenerator.GenerateLevel();
            playCount++;
        }
        else
        {
            level = AdaptiveMapGenerator.GenerateLevel();
        }

        GenerateBase();
        PrintMatrix(level);
    }

    private void Update()
    {
        if (player.transform.position.x >= endTile.transform.position.x - 0.3f &&
            player.transform.position.x <= endTile.transform.position.x + 0.3f &&
            player.transform.position.y >= endTile.transform.position.y - 0.3f &&
            player.transform.position.y <= endTile.transform.position.y + 0.3f)
        {
            GameManager.Instance.LevelEnded(LevelEndCondition.LevelCompleted);
        }
    }

    void GenerateBase()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                switch (level[i, j])
                {
                    case 0:
                        spawnTile(i, j);
                        break;
                    case 1:
                        spawnTile(i, j);
                        player.transform.position = new Vector3(j, i);
                        break;
                    case 2:
                        endTile = Instantiate(tile, new Vector3(j, i), Quaternion.identity);
                        endTile.SingleColor(Color.green);
                        break;
                    case 3:
                        var spawnedSlowTile = Instantiate(slowTile, new Vector3(j, i), Quaternion.identity);
                        spawnedSlowTile.name = $"Tile {i} {j}";
                        break;
                    case 4:
                        spawnTile(i, j);
                        var spawnedEnemy = Instantiate(enemy, new Vector3(j, i), Quaternion.identity);
                        spawnedEnemy.name = $"Enemy {i} {j}";
                        break;
                    default:
                        var spawnedWalls = Instantiate(wall, new Vector3(j, i), Quaternion.identity);
                        spawnedWalls.name = $"Wall {i} {j}";
                        spawnedWalls.SingleColor(Color.black);
                        break;


                }

            }
        }

        cam.transform.position = new Vector3 ((float) 18/2 - 0.5f, (float) 11/2 - 0.5f, -10);

    }

    private void spawnTile(int i, int j)
    {
        var spawnedStartTile = Instantiate(tile, new Vector3(j, i), Quaternion.identity);
        spawnedStartTile.name = $"Tile {i} {j}";
        spawnedStartTile.PatternColor(i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0);
    }

    

    void PrintMatrix(int[,] matrix)
    {
        string output = "Level Matrix:\n";

        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                output += matrix[row, col] + " "; // Add space between numbers
            }
            output += "\n"; // New line after each row
        }

        Debug.Log(output);
    }
}
