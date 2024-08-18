using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCanvasSettings : MonoBehaviour
{
    PlayerMovement player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    public void SetSpeedState(int stateID)
    {
        player.SelectSpeedState(stateID);
    }

    public void RestartLevel()
    {
        player.RestartLevel();
    }
}
