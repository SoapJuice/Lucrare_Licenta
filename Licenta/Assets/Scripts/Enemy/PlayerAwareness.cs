using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarnes : MonoBehaviour
{
    public bool isAware {  get; private set; }

    public Vector2 directionToPlayer {  get; private set; }

    [SerializeField]
    private float playerAwarenessDistance;

    private Transform player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player_Movement>().transform;
    }

    void Update()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        directionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= playerAwarenessDistance )
        {
            isAware = true;
        }
        else
        {
            isAware = false;
        }
    }
}
