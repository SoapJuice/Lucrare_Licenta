using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private float totalTime;
    private float remainingTime;

    private void Start()
    {
        remainingTime = totalTime;
    }
    void Update()
    {
        remainingTime -= Time.deltaTime;
        int second = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{00}", second);

        GameManager.Instance.levelStats.remainingTime = GetTimeRemainingPercentage();



        if (remainingTime <= 0.1 )
        {
            GameManager.Instance.LevelEnded();
        }
    }

    public float GetTimeRemainingPercentage()
    {
        if (totalTime <= 0)
        {
            return 0f;
        }
        return (remainingTime / totalTime) * 100f;
    }
}
