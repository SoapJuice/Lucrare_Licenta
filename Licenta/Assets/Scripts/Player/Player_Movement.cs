using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private float defaultSpeed = 4f;
    [SerializeField]
    private float rotationSpeed;


    private float currentSpeed;
    private Rigidbody2D player;
    private Vector2 movementInput;
    private Vector2 smoothMovement;
    private Vector2 smoothMovementVelocity;

    private void Awake()
    {
        currentSpeed = defaultSpeed;
        player = GetComponent<Rigidbody2D>();

        
    }
    private void FixedUpdate()
    {
        player.velocity = movementInput * currentSpeed;
        rotationHandler();
    }


    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    public void speedModifier(float modifier)
    {
        float newSpeed = defaultSpeed * modifier;
        currentSpeed = newSpeed;
    }

    public void speedReset()
    {
        currentSpeed = defaultSpeed;
    }

    public void rotationHandler()
    {
        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, movementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            player.MoveRotation(rotation);
        }
    }
}
