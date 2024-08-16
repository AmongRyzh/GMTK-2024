using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float regularMoveSpeed, highMoveSpeed;
    private Rigidbody2D rb;

    private enum SpeedState
    {
        Paused = 0,
        RegularSpeed = 1,
        HighSpeed = 2
    }
    private SpeedState speedState;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        speedState = SpeedState.RegularSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (speedState == SpeedState.RegularSpeed || speedState == SpeedState.HighSpeed)
            {
                speedState = SpeedState.Paused;
            }
            else
            {
                speedState = SpeedState.RegularSpeed;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            speedState = speedState == SpeedState.HighSpeed ? SpeedState.RegularSpeed : SpeedState.HighSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void FixedUpdate()
    {
        rb.isKinematic = speedState == SpeedState.Paused;

        if (speedState == SpeedState.Paused) return;

        float moveSpeed = speedState == SpeedState.RegularSpeed ? regularMoveSpeed : highMoveSpeed;

        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    public void SelectSpeedState(int stateID)
    {
        speedState = (SpeedState)stateID;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
