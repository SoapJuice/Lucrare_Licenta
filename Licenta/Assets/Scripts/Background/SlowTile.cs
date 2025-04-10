using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTile : MonoBehaviour
{
    [SerializeField]
    private float slow = 0.5f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_Movement player = collision.GetComponent<Player_Movement>();
            if (player != null)
            {
                player.speedModifier(slow);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_Movement player = collision.GetComponent<Player_Movement>();
            if (player != null)
            {
                player.speedReset();
            }
        }
    }
}
