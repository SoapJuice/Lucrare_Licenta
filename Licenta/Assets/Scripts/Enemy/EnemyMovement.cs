using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;

    private Rigidbody2D enemy;
    private PlayerAwarnes playerAwarnes;
    private Vector2 targetDirection;
    private void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
        playerAwarnes = GetComponent<PlayerAwarnes>();
    }
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if(playerAwarnes.isAware)
        {
            targetDirection = playerAwarnes.directionToPlayer;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        if (targetDirection == Vector2.zero)
        {
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        enemy.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        if (targetDirection == Vector2.zero) 
        {
            enemy.velocity = Vector2.zero;
        }
        else
        {
            enemy.velocity = transform.up * speed;
        }
    }
}
