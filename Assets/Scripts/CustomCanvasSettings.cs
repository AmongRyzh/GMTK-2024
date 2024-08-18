using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCanvasSettings : MonoBehaviour
{
    public void SetSpeedState(int stateID)
    {
        FindObjectOfType<PlayerMovement>().SelectSpeedState(stateID);
    }

    public void RestartLevel()
    {
        FindObjectOfType<PlayerMovement>().RestartLevel();
    }
}
