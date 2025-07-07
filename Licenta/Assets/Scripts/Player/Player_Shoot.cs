using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private Transform gunOffset;

    [SerializeField]
    private float delay;

    private bool fireContinuously;

    private float lastFireTime;

    void Update()
    {
        if (fireContinuously)
        {
            float timeSinceLastFire = Time.time - lastFireTime;

            if (timeSinceLastFire >= delay)
            {
                FireBullet();
                lastFireTime = Time.time;
            }

        }
    }

    private void FireBullet()
    {
        GameObject spawnedBullet = Instantiate(bullet, gunOffset.position, transform.rotation);
        Rigidbody2D rigidbody2D = spawnedBullet.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = bulletSpeed * transform.up;
    }


    private void OnFire(InputValue inputValue)
    {
        fireContinuously = inputValue.isPressed;
    }
}
