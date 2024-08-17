using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        transform.localScale = new Vector2(player.transform.localScale.x < 0 ? 2 : -2, transform.localScale.y);
    }
}
