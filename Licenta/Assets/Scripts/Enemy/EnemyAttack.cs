using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float damange;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player_Movement>())
        {
            var health = collision.gameObject.GetComponent<HealthController>();
            health.TakeDamage(damange);
        }
    }
}
