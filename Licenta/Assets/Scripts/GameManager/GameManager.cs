using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Stats levelStats = new Stats();

    public Stats savedStats = new Stats();

    public string difficulty = "";

    public int playerType = 0;

    public SimpleRLNeuralNetwork neuralNetwork;



    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            neuralNetwork = new SimpleRLNeuralNetwork(new int[] { 7, 10, 3 });
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LevelEnded()
    {
        GameManager.Instance.savedStats = levelStats;

        GameManager.Instance.savedStats.rooms += 1;
        SceneManager.LoadScene("Game");

        //Debug.Log("remaining health: " + savedStats.remainingPlayerHealth);
        //Debug.Log("remaining time: " + savedStats.remainingTime);
        //Debug.Log("enemy: " + savedStats.enemysKilled);
        //Debug.Log("total enemy kills: " + savedStats.totalEnemysKilled);
        //Debug.Log("rooms " + savedStats.rooms);

    }

    public void PrintHistory()
    {
        var hist = savedStats.history;
        if (hist == null || hist.Count == 0)
        {
            Debug.Log("History is empty.");
            return;
        }

        for (int i = 0; i < hist.Count; i++)
        {
            Vector3 v = hist[i];
            Debug.Log($"Run #{i + 1}: TimeRatio={v.x:F2}, HPRatio={v.y:F2}, KillRatio={v.z:F2}");
        }
    }
}

public class Stats
{
    public float remainingPlayerHealth;

    public float remainingTime;

    public int totalEnemys = 3;
    public int totalEnemysKilled;
    public int enemysKilled;

    public int rooms;

    public List<(float[] input, float reward)> policyHistory = new List<(float[], float)>();


    public List<Vector3> history;

    public Stats()
    {
        history = new List<Vector3>();
    }


}
