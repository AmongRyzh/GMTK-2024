using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

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

    [SerializeField] private float scaleDiff, scaleDuration/*, gravityDiff*/;

    [SerializeField] private Slider scaleSlider;
    private TMP_Text scaleText;

    [SerializeField] private float jumpForce;

    [SerializeField] private Transform deathCheck;
    [SerializeField] private LayerMask deathLayer;

    Vector2 savedVelocity;

    /*bool isGrounded;
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;*/

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scaleSlider = FindObjectOfType<Slider>();
        scaleText = scaleSlider.transform.Find("Handle Slide Area").Find("Handle").GetComponentInChildren<TMP_Text>();

        speedState = SpeedState.RegularSpeed;
    }

    private void Update()
    {
        //isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (speedState == SpeedState.RegularSpeed || speedState == SpeedState.HighSpeed)
            {
                speedState = SpeedState.Paused;
                savedVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
            }
            else
            {
                speedState = SpeedState.RegularSpeed;
                rb.velocity = savedVelocity;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (speedState == SpeedState.Paused)
                rb.velocity = savedVelocity;
            speedState = speedState == SpeedState.HighSpeed ? SpeedState.RegularSpeed : SpeedState.HighSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (canBeScaled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(ScaleCharacter(true));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                StartCoroutine(ScaleCharacter(false));
            }
        }
    }

    private void FixedUpdate()
    {
        scaleSlider.value = transform.localScale.y;
        scaleText.text = $"{scaleSlider.value}";

        rb.isKinematic = speedState == SpeedState.Paused;

        if (speedState == SpeedState.Paused) return;

        float moveSpeed = speedState == SpeedState.RegularSpeed ? regularMoveSpeed : highMoveSpeed;

        rb.velocity = new Vector2(moveSpeed * (transform.localScale.x / 2), rb.velocity.y);
    }

    public void SelectSpeedState(int stateID)
    {
        speedState = (SpeedState)stateID;
        switch (stateID)
        {
            case 0:
                savedVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
                break;
            case 1:
                break;
                rb.velocity = savedVelocity;
            case 2:
                break;
                rb.velocity = savedVelocity;
            default:
                break;
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        yield return new WaitUntil(() => speedState != SpeedState.Paused);
        if (other.CompareTag("Jump Pad")/* && speedState != SpeedState.Paused*/)
        {
            other.GetComponent<Animator>().SetTrigger("JumpPad");
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (other.CompareTag("Player Rotator")/* && speedState != SpeedState.Paused*/)
        {
            other.GetComponent<Animator>().SetTrigger("PlayerRotator");
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    bool IsCollidingRespawn()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(deathCheck.position, new Vector2(.1f, 3.8f * (transform.localScale.y / 2)), 0, deathLayer);
        return colliders.Length > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCollidingRespawn() || collision.collider.CompareTag("Respawn"))
        {
            RestartLevel();
        }
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

        Vector2 newPositiveYScale;
        Vector2 newNegativeYScale;

        if (transform.localScale.x > 0)
        {
            newPositiveYScale = new Vector2(transform.localScale.x - scaleDiff, transform.localScale.y + scaleDiff);
            newNegativeYScale = new Vector2(transform.localScale.x + scaleDiff, transform.localScale.y - scaleDiff);
        }
        else
        {
            newPositiveYScale = new Vector2(transform.localScale.x + scaleDiff, transform.localScale.y + scaleDiff);
            newNegativeYScale = new Vector2(transform.localScale.x - scaleDiff, transform.localScale.y - scaleDiff);
        }

        //float newGravityScale = positiveYScale == true ? rb.gravityScale - gravityDiff : rb.gravityScale + gravityDiff;

        Vector2 newPlayerScale = positiveYScale == true ? newPositiveYScale : newNegativeYScale;

        if ((newPlayerScale.y > 3.8f || newPlayerScale.y < 0.2f)/* && (newGravityScale > 1.6f || newGravityScale < 0.4f)*/)
        {
            canBeScaled = true;
            yield break;
        }

        //DOTween.To(() => rb.gravityScale, x => rb.gravityScale = x, newGravityScale, 0.2f);

        if (newPlayerScale.y < transform.localScale.y)
            transform.DOMoveY(transform.position.y - scaleDiff, scaleDuration);
        else
            transform.DOMoveY(transform.position.y + scaleDiff, scaleDuration);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(deathCheck.position, new Vector2(.1f, 3.8f * (transform.localScale.y / 2)));
    }
}
