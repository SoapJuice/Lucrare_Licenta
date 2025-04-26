using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapGenerator : MonoBehaviour
{
    public static int[,] GenerateLevel(LevelResult results)
    {
        int rows = 11, cols = 18;
        int[,] matrix = new int[rows, cols];
        for (int x = 0; x < cols; x++)
        {
            matrix[0, x] = 9;
            matrix[rows - 1, x] = 9;
        }
        for (int y = 0; y < rows; y++)
        {
            matrix[y, 0] = 9;
            matrix[y, cols - 1] = 9;
        }
        matrix[5, 1] = 1;
        matrix[5, cols - 2] = 2;

        for (int i = 0; i < 15; i++)
        {
            int x = Random.Range(1, cols - 1);
            int y = Random.Range(1, rows - 1);
            if (matrix[y, x] == 0) matrix[y, x] = 9;
        }
        for (int i = 0; i < 8; i++)
        {
            int x = Random.Range(1, cols - 1);
            int y = Random.Range(1, rows - 1);
            if (matrix[y, x] == 0) matrix[y, x] = 3;
        }
        for (int i = 0; i < 3; i++)
        {
            int x = Random.Range(1, cols - 1);
            int y = Random.Range(1, rows - 1);
            if (matrix[y, x] == 0) matrix[y, x] = 4;
        }

        return matrix;
    }
}
