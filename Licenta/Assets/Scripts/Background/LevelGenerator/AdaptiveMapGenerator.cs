using UnityEngine;

public class AdaptiveMapGenerator
{
    private float[] obstacleRatios;
    private int rows = 11;
    private int cols = 18;
    private int totalObstacles = 40;
    public float[] LastState { get; private set; }
    public int LastAction { get; private set; }



    public AdaptiveMapGenerator()
    {
        GenerateObstacleRatios();
    }

    public int[,] GenerateLevel()
    {
        int[,] map = new int[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (r == 0 || r == rows - 1 || c == 0 || c == cols - 1)
                    map[r, c] = 9;
                else
                    map[r, c] = 0;
            }
        }

        map[5, 1] = 1;  
        map[5, 16] = 2;  

        float pSlow = obstacleRatios[0];
        float pEnemy = obstacleRatios[1];
        float pWall = obstacleRatios[2];

        

        int slowCount = Mathf.RoundToInt(totalObstacles * pSlow);
        int enemyCount = Mathf.RoundToInt(totalObstacles * pEnemy);
        int wallCount = Mathf.RoundToInt(totalObstacles * pWall);

        System.Random rand = new System.Random();

        void PlaceTiles(int tileNumber, int count)
        {
            int placed = 0;
            while (placed < count)
            {
                int r = rand.Next(1, rows - 1);
                int c = rand.Next(1, cols - 1);

                if (map[r, c] == 0)
                {
                    map[r, c] = tileNumber;
                    placed++;
                }
            }
        }

        GameManager.Instance.levelStats.totalEnemys = enemyCount;


        PlaceTiles(3, slowCount);
        PlaceTiles(4, enemyCount);
        PlaceTiles(9, wallCount);

        return map;
    }

    private void GenerateObstacleRatios()
    {
        var stats = GameManager.Instance.savedStats;

        string difficulty = EvaluateDecisionTree(stats);
        int clusterIndex = EvaluateDataCluster(stats);

        if(difficulty == "H")
        {
            totalObstacles = 40;
        }
        else if (difficulty == "N")
        {
            totalObstacles = 30;
        }
        else
        {
            totalObstacles = 20;
        }

        GameManager.Instance.playerType = clusterIndex;

        GameManager.Instance.difficulty = difficulty;

        var net = GameManager.Instance.neuralNetwork;
        float[] input = NNBuildInput(stats, difficulty, clusterIndex);

        obstacleRatios = net.Predict(input);

        float reward = CalculateReward(stats, clusterIndex);

        net.TrainWithReward(reward);

        GameManager.Instance.savedStats.policyHistory.Add((input, reward));

        Debug.Log("Difficulty: " + difficulty);
    }

    float CalculateReward(Stats stats, int index)
    {
        float hpRatio = stats.remainingPlayerHealth;
        float timeRatio = stats.remainingTime/100;
        float killRatio = (float)stats.enemysKilled / stats.totalEnemys;

        switch (index)
        {
            case 1:
                return killRatio * 0.4f + hpRatio * 0.3f + timeRatio * 0.3f;
            case 2:
                return hpRatio * 0.4f + killRatio * 0.3f + timeRatio * 0.3f;
            case 0:
            default:
                return hpRatio * 0.3f + killRatio * 0.3f + timeRatio * 0.4f;
        }
    }

    private int EvaluateDataCluster(Stats stats)
    {
        KMeans kmeans = new KMeans();
        Vector3 playerStats = new Vector3(stats.remainingPlayerHealth * 100, stats.remainingTime, (float)stats.enemysKilled / stats.totalEnemys * 100);
        int clusterIndex = 0;

        if (stats.rooms > 3)
        {
            kmeans.Fit(GameManager.Instance.savedStats.history, k: 3, maxIterations: 15);
            clusterIndex = kmeans.Predict(playerStats);
            Debug.Log("Player Type: " + clusterIndex + kmeans.GetClusterLabel(clusterIndex));

        }
        GameManager.Instance.savedStats.history.Add(playerStats);

        return clusterIndex;
    }
    private string EvaluateDecisionTree(Stats stats)
    {
        float killRatio = 0f;
        if (stats.totalEnemys != 0)
        {
            killRatio = (float)stats.enemysKilled / stats.totalEnemys * 100;
        }

        Debug.Log("health " + stats.remainingPlayerHealth*100 + " remaining time: " + stats.remainingTime + " kill ratio: " + killRatio);

        Node root = new DecisionBranch(
            () => stats.remainingPlayerHealth * 100 > 50,
            new DecisionBranch(
                () => stats.remainingTime < 30,
                new DecisionNode("N"),
                new DecisionBranch(
                    () => killRatio > 50,
                    new DecisionNode("H"),
                    new DecisionNode("N"))
            ),
            new DecisionBranch(
                () => stats.remainingTime < 30,
                new DecisionNode("E"),
                new DecisionBranch(
                    () => killRatio > 50,
                    new DecisionNode("N"),
                    new DecisionNode("E"))
            )
        );
        return root.Evaluate();
    }

    public static float[] NNBuildInput(Stats stats, string difficulty, int playerIndex)
    {
        float difficultyIndex = difficulty switch
        {
            "E" => 0f,
            "N" => 0.5f,
            "H" => 1f,
            _ => 0f
        };

        float health = Mathf.Clamp01(stats.remainingPlayerHealth);
        float time = Mathf.Clamp01(stats.remainingTime / 100f);
        float killRatio = stats.totalEnemys > 0
            ? Mathf.Clamp01((float)stats.enemysKilled / stats.totalEnemys)
            : 0f;

        
        float[] playerType = new float[3];
        if (playerIndex >= 0 && playerIndex <= 2)
            playerType[playerIndex] = 1f;

        
        return new float[]
        {
            health, time, killRatio, difficultyIndex,
            playerType[0], playerType[1], playerType[2]
        };
    }

}