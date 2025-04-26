using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Rendering.CameraUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Stats levelStats {  get; private set; }
    public LevelResult lastLevelResult { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            levelStats = new Stats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LevelEnded(LevelEndCondition condition)
    {
        lastLevelResult = new LevelResult(condition, levelStats.remainingPlayerHealth, levelStats.remainingTime, levelStats.enemyKills);
        Debug.Log(condition);
        Debug.Log(levelStats.remainingPlayerHealth);
        Debug.Log(levelStats.remainingTime);
        Debug.Log(levelStats.enemyKills);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public class Stats
{
    public float remainingPlayerHealth;
    public float remainingTime;
    public float enemyKills;
}
public enum LevelEndCondition
{
    PlayerDeath,
    TimeOut,
    LevelCompleted
}
public struct LevelResult
{
    public LevelEndCondition endCondition;
    public float healthRemaining;
    public float timeRemaining;
    public float enemyKills;

    public LevelResult(LevelEndCondition condition, float health, float time, float kills)
    {
        endCondition = condition;
        healthRemaining = health;
        timeRemaining = time;
        enemyKills = kills;
    }
}
