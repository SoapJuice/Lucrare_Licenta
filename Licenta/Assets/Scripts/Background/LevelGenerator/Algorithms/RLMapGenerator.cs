using UnityEngine;
using System.Collections.Generic;
using static PlayerClusterAnalyzer;

public static class RLMapGenerator
{
    private static NeuralNetwork model = new NeuralNetwork();

    public static int[,] GenerateMap(string difficulty, PlayerType playerType, Stats stats)
    {
        int rows = 11, cols = 18;
        int[,] map = new int[rows, cols];

        // Set border walls
        for (int x = 0; x < cols; x++) { map[0, x] = 9; map[rows - 1, x] = 9; }
        for (int y = 0; y < rows; y++) { map[y, 0] = 9; map[y, cols - 1] = 9; }

        // Place start and goal
        map[5, 1] = 1;
        map[5, cols - 2] = 2;

        float difficultyValue = difficulty switch
        {
            "Easy" => 0f,
            "Medium" => 0.5f,
            "Hard" => 1f,
            _ => 0.5f
        };
        float playerTypeValue = GetPlayerTypeValue(playerType);

        // Normalize stats (assuming 0–100% for health/time, max 20 kills)
        float healthNorm = Mathf.Clamp01(stats.remainingPlayerHealth / 100f);
        float timeNorm = Mathf.Clamp01(stats.remainingTime / 100f);
        float killsNorm = Mathf.Clamp01(stats.enemyKills / 10f);

        for (int y = 1; y < rows - 1; y++)
        {
            for (int x = 1; x < cols - 1; x++)
            {
                if (map[y, x] == 0)
                {
                    float[] inputs = new float[]
                    {
                        difficultyValue,
                        (float)x / cols,
                        (float)y / rows,
                        playerTypeValue,
                        healthNorm,
                        timeNorm,
                        killsNorm
                    };

                    float value = model.Predict(inputs);
                    map[y, x] = SampleTile(value);
                }
            }
        }

        return map;
    }

    private static float GetPlayerTypeValue(PlayerType playerType)
    {
        return playerType switch
        {
            PlayerType.Aggressive => 0f,
            PlayerType.Fast => 0.5f,
            PlayerType.Safe => 1f,
            _ => 0.5f
        };
    }

    private static int SampleTile(float value)
    {
        if (value < 0.1f) return 0;   // walkable
        if (value < 0.3f) return 3;   // slow
        if (value < 0.4f) return 4;   // enemy
        if (value < 0.6f) return 9;   // wall
        return 0;
    }

    // Called after gameplay ends
    public static void Train(float reward)
    {
        model.Train(reward);
    }

    public class NeuralNetwork
    {
        private float[] weights = new float[7];

        public NeuralNetwork()
        {
            for (int i = 0; i < weights.Length; i++)
                weights[i] = Random.value;
        }

        public float Predict(float[] inputs)
        {
            float sum = 0f;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += inputs[i] * weights[i];
            }
            return Sigmoid(sum);
        }

        public void Train(float reward)
        {
            float lr = 0.1f;
            for (int i = 0; i < weights.Length; i++)
            {
                float noise = (Random.value - 0.5f) * 2f;
                weights[i] += lr * reward * noise;
            }
        }

        private float Sigmoid(float x)
        {
            return 1f / (1f + Mathf.Exp(-x));
        }
    }
}
