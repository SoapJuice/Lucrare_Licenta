using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float currentHeatlh;

    private float healthPercentage
    {
        get
        {
            return currentHeatlh / maxHealth;
        }
    }

    public UnityEvent Damaged;

    public UnityEvent HealthChanged;
    public bool immune { get; set; }

    private void Start()
    {
        currentHeatlh = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        if (currentHeatlh == 0 ) 
        {
            return;
        }
        if (immune)
        {
            return;
        }
        currentHeatlh -= damage;

        GameManager.Instance.levelStats.remainingPlayerHealth = getHealthPercentage();

        HealthChanged.Invoke();

        if (currentHeatlh <= 0)
        {
            currentHeatlh = 0;
            GameManager.Instance.LevelEnded(LevelEndCondition.PlayerDeath);
        }
        else
        {
            Damaged.Invoke();
        }
    }

    public float getHealthPercentage()
    {
        return healthPercentage;
    }

}
