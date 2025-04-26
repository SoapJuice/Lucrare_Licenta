using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    void PlayerDied()
    {
        GameManager.Instance.LevelEnded(LevelEndCondition.PlayerDeath);
    }
    void RandOutOfTime()
    {
        GameManager.Instance.LevelEnded(LevelEndCondition.TimeOut);
    }
    void PlayerWin()
    {
        GameManager.Instance.LevelEnded(LevelEndCondition.LevelCompleted);
    }
}
