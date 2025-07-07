using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statisticsText;

    private void Start()
    {
        string statsMessage =
            "Enemies Killed: " + GameManager.Instance.savedStats.enemysKilled + "\n" +
            "Total Enemies Killed: " + GameManager.Instance.savedStats.totalEnemysKilled + "\n" +
            "Rooms Cleared: " + GameManager.Instance.savedStats.rooms + "\n" +
            "Room Difficulty: " + GameManager.Instance.difficulty + "\n" +
            "Player Type: " + GameManager.Instance.playerType;

        statisticsText.text = statsMessage;
    }

    private void Update()
    {
        string statsMessage =
            "Enemies Killed: " + GameManager.Instance.savedStats.enemysKilled + "\n" +
            "Total Enemies Killed: " + GameManager.Instance.savedStats.totalEnemysKilled + "\n" +
            "Rooms Cleared: " + GameManager.Instance.savedStats.rooms + "\n" +
            "Room Difficulty: " + GameManager.Instance.difficulty + "\n" +
            "Player Type: " + GameManager.Instance.playerType;

        statisticsText.text = statsMessage;
    }

}
