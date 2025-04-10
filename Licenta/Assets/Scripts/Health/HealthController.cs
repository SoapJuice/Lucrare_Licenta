using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float currentHeatlh;
    [SerializeField]
    private float maxHealth;

    private float healthPercentage
    {
        get
        {
            return currentHeatlh / maxHealth;
        }
    }

    public UnityEvent Died;

    public UnityEvent Damaged;
    public bool immune { get; set; }
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

        if (currentHeatlh <= 0)
        {
            currentHeatlh = 0;
            Died.Invoke();
        }
        else
        {
            Damaged.Invoke();
        }
    }

}
