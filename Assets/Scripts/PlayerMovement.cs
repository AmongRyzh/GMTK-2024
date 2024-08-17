using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    private bool canBeScaled = true;

    [SerializeField] private float scaleDiff, scaleDuration;

    [SerializeField] private Slider scaleSlider;

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

        if (canBeScaled)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(ScaleCharacter(true));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(ScaleCharacter(false));
            }
        }
    }

    private void FixedUpdate()
    {
        rb.gravityScale = System.Convert.ToInt32(speedState != SpeedState.Paused);

        if (speedState == SpeedState.Paused) return;

        float moveSpeed = speedState == SpeedState.RegularSpeed ? regularMoveSpeed : highMoveSpeed;

        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        scaleSlider.value = transform.localScale.y;
    }

    public void SelectSpeedState(int stateID)
    {
        speedState = (SpeedState)stateID;
    }

    private IEnumerator ScaleCharacter(bool positiveYScale)
    {
        if (transform.localScale.y < 0.2f)
        {
            transform.localScale = new Vector2(3.8f, 0.2f);
            yield break;
        }
        else if (transform.localScale.y > 3.8f)
        {
            transform.localScale = new Vector2(0.2f, 3.8f);
            yield break;
        }

        canBeScaled = false;

        Vector2 newPositiveYScale = new Vector2(transform.localScale.x - scaleDiff, transform.localScale.y + scaleDiff);
        Vector2 newNegativeYScale = new Vector2(transform.localScale.x + scaleDiff, transform.localScale.y - scaleDiff);

        Vector2 newPlayerScale = positiveYScale == true ? newPositiveYScale : newNegativeYScale;

        if (newPlayerScale.y > 3.8f || newPlayerScale.y < 0.2f)
        {
            canBeScaled = true;
            yield break;
        }


        if (newPlayerScale.y < transform.localScale.y)
            transform.DOMoveY(transform.position.y - scaleDiff, scaleDuration);
        transform.DOScale(newPlayerScale, scaleDuration);
        yield return new WaitForSeconds(scaleDuration);

        canBeScaled = true;
    }

    public void ScaleCharacter(float newYScale)
    {
        print($"SCALECHARACTER({newYScale}) CALLED");
        if (canBeScaled)
        {
            if (newYScale > 2)
            {
                float scaleSubtracter = newYScale - 2;
                transform.localScale = new Vector2(2 - scaleSubtracter, newYScale);
            }
            else if (newYScale < 2)
            {
                float scaleSubtracter = 2 - newYScale;
                transform.localScale = new Vector2(2 + scaleSubtracter, newYScale);
                //transform.position = new Vector2(transform.position.x, transform.position.y - scaleSubtracter);
            }
            else
            {
                transform.localScale = new Vector2(2, 2);
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
