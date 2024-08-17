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
        transform.localScale = new Vector2(-FindObjectOfType<PlayerMovement>().transform.localScale.x, transform.localScale.y);
    }
}
