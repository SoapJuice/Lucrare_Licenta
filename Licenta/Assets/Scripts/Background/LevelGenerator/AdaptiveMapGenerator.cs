using UnityEngine;

public static class AdaptiveMapGenerator
{
    private static DecisionTree difficultyTree;
    private const bool DEBUG_MODE = true;

    static AdaptiveMapGenerator()
    {
        InitializeDecisionTree();
    }

    private static void InitializeDecisionTree()
    {
        difficultyTree = new DecisionTree();

        var easyMap = new DecisionTree.Node("Generate Easy Map");
        var mediumMap = new DecisionTree.Node("Generate Medium Map");
        var hardMap = new DecisionTree.Node("Generate Hard Map");

        difficultyTree.AddNode(
            "IsAggressive",
            new DecisionTree.Node(
                "HealthLow",
                hardMap,
                mediumMap
            ),
            new DecisionTree.Node(
                "IsFast",
                new DecisionTree.Node(
                    "TimeLow",
                    hardMap,
                    mediumMap
                ),
                new DecisionTree.Node(
                    "KillsHigh",
                    mediumMap,
                    easyMap
                )
            )
        );
    }

    public static int[,] GenerateLevel()
    {

        PlayerClusterAnalyzer.AddLevelResult(GameManager.Instance.lastLevelResult);

        // Get current stats
        var stats = GameManager.Instance.levelStats;
        var playerType = PlayerClusterAnalyzer.ClassifyCurrentPlayer();


        string difficulty = difficultyTree.Evaluate(stats);

        int[,] map;
        int attempts = 0;
        do
        {
            map = RLMapGenerator.GenerateMap(difficulty, playerType, stats);
            
            attempts++;
        } while (!Pathfinder.IsMapCompletable(map) && attempts < 5);

        if (DEBUG_MODE)
        {
            Debug.Log($"[Map Generation]\nDifficulty: {difficulty}\nAttempts: {attempts}");
        }

        if (DEBUG_MODE)
        {
            Debug.Log($"[Map Generation]\n" +
                     $"Player Type: {playerType}\n" +
                     $"Health: {stats.remainingPlayerHealth}%\n" +
                     $"Time: {stats.remainingTime}%\n" +
                     $"Kills: {stats.enemyKills}/20\n" +
                     $"Decision: {difficulty}");
        }



        return map;
    }

    // Call this from debug console
    public static void DebugClusteringData()
    {
        Debug.Log(PlayerClusterAnalyzer.GetDebugString());
    }
}