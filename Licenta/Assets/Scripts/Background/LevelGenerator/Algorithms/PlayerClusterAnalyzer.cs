using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class PlayerClusterAnalyzer
{
    public enum PlayerType { Safe, Aggressive, Fast }

    private static List<LevelResult> historicalData = new List<LevelResult>();
    private const int MAX_ENEMIES = 5;
    private const bool DEBUG_MODE = true;

    public static void AddLevelResult(LevelResult result)
    {
        if (result.endCondition == LevelEndCondition.LevelCompleted)
        {
            historicalData.Add(result);

            if (DEBUG_MODE)
            {
                Debug.Log($"Added level result:\n" +
                         $"- Health: {result.healthRemaining}%\n" +
                         $"- Time: {result.timeRemaining}%\n" +
                         $"- Kills: {result.enemyKills}/20\n" +
                         $"- Condition: {result.endCondition}");
            }
        }
    }

    public static PlayerType ClassifyCurrentPlayer()
    {
        var stats = GameManager.Instance.levelStats;
        return ClassifyPlayer(stats.remainingPlayerHealth, stats.remainingTime, stats.enemyKills);
    }

    private static PlayerType ClassifyPlayer(float healthPercent, float timePercent, float kills)
    {
        float normKills = Mathf.Clamp01(kills / MAX_ENEMIES);
        Vector3 currentPlayer = new Vector3(healthPercent / 100f, timePercent / 100f, normKills);

        Vector3[] centroids = CalculateCentroids();

        if (DEBUG_MODE)
        {
            Debug.Log("Current player stats (normalized):\n" +
                     $"Health: {currentPlayer.x:0.00} (" + Mathf.RoundToInt(healthPercent) + "%)\n" +
                     $"Time: {currentPlayer.y:0.00} (" + Mathf.RoundToInt(timePercent) + "%)\n" +
                     $"Kills: {currentPlayer.z:0.00} (" + kills + "/20)");
        }

        // Calculate distances to each centroid
        float[] distances = new float[3];
        for (int i = 0; i < 3; i++)
        {
            distances[i] = Vector3.Distance(currentPlayer, centroids[i]);
        }

        if (DEBUG_MODE)
        {
            Debug.Log("Distance to centroids:\n" +
                     $"Safe ({centroids[0]}): {distances[0]:0.00}\n" +
                     $"Aggressive ({centroids[1]}): {distances[1]:0.00}\n" +
                     $"Fast ({centroids[2]}): {distances[2]:0.00}");
        }

        // Return the closest cluster
        if (distances[0] < distances[1] && distances[0] < distances[2])
            return PlayerType.Safe;
        return distances[1] < distances[2] ? PlayerType.Aggressive : PlayerType.Fast;
    }

    private static Vector3[] CalculateCentroids()
    {
        if (historicalData.Count < 3)
        {
            return GetDefaultCentroids();
        }

        List<Vector3>[] clusters = new List<Vector3>[3];
        for (int i = 0; i < 3; i++) clusters[i] = new List<Vector3>();

        // Cluster historical data
        foreach (var data in historicalData)
        {
            Vector3 point = new Vector3(
                data.healthRemaining / 100f,
                data.timeRemaining / 100f,
                Mathf.Clamp01(data.enemyKills / MAX_ENEMIES));

            if (data.healthRemaining > 70f && data.timeRemaining > 60f)
                clusters[0].Add(point);
            else if (data.enemyKills > 3f)
                clusters[1].Add(point);
            else
                clusters[2].Add(point);
        }

        // Calculate final centroids
        Vector3[] centroids = new Vector3[3];
        for (int i = 0; i < 3; i++)
        {
            centroids[i] = clusters[i].Count > 0 ?
                CalculateMean(clusters[i]) :
                GetDefaultCentroids()[i];
        }

        return centroids;
    }

    private static Vector3[] GetDefaultCentroids()
    {
        return new Vector3[]
        {
            new Vector3(0.8f, 0.8f, 0.2f),    // Safe (high health/time, few kills)
            new Vector3(0.4f, 0.5f, 0.9f),    // Aggressive (medium health/time, high kills)
            new Vector3(0.6f, 0.3f, 0.6f)      // Fast (medium health, low time, medium kills)
        };
    }

    private static Vector3 CalculateMean(List<Vector3> points)
    {
        Vector3 sum = Vector3.zero;
        foreach (var p in points) sum += p;
        return sum / points.Count;
    }

    public static string GetDebugString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== PLAYER CLUSTERING DATA ===");
        sb.AppendLine($"Stored level results: {historicalData.Count}");

        if (historicalData.Count > 0)
        {
            sb.AppendLine("\nRecent results:");
            foreach (var r in historicalData.TakeLast(3))
            {
                sb.AppendLine($"- H:{r.healthRemaining}% T:{r.timeRemaining}% K:{r.enemyKills}/20");
            }

            var current = GameManager.Instance.levelStats;
            sb.AppendLine("\nCurrent classification:");
            sb.AppendLine($"- Health: {current.remainingPlayerHealth}%");
            sb.AppendLine($"- Time: {current.remainingTime}%");
            sb.AppendLine($"- Kills: {current.enemyKills}/20");
            sb.AppendLine($"- Type: {ClassifyCurrentPlayer()}");
        }

        return sb.ToString();
    }
}