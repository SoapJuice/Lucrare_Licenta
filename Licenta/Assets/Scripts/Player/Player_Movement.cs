using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f;

    private Rigidbody2D player;
    private Vector2 movementInput;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        
    }
    private void FixedUpdate()
    {
        player.velocity = movementInput * 4f;
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }
}
