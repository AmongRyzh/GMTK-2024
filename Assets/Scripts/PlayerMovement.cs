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

    public enum SpeedState
    {
        Paused = 0,
        RegularSpeed = 1,
        HighSpeed = 2
    }
    public SpeedState speedState { get; private set; }

    private bool canBeScaled = true;

    [SerializeField] private Transform scaleUpPossibilityCheck;
    [SerializeField] private Transform scaleDownPossibilityCheck;
    [SerializeField] private float scaleCheckWidth;
    [SerializeField] private float scaleDiff, scaleDuration/*, gravityDiff*/;

    [SerializeField] private Slider scaleSlider;
    private TMP_Text scaleText;

    [SerializeField] private float jumpForce;

    [SerializeField] private Transform deathCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask hydraulicPressLayer;
    [SerializeField] private float deathCheckHeight;

    Vector2 savedVelocity;
    Tween currentTween;

    [HideInInspector] public bool canChangeSpeed = true;

    private bool isCollidingNoScaleChangeZone;

    [SerializeField] private GameObject screenOff;

    Animator anim;

    /*bool isGrounded;
    [SerializeField] Transform feetPos;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;*/

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scaleSlider = FindObjectOfType<Slider>();
        scaleText = scaleSlider.transform.Find("Handle Slide Area").Find("Handle").GetComponentInChildren<TMP_Text>();
        scaleText.maxVisibleCharacters = 4;

        anim = GetComponent<Animator>();

        speedState = SpeedState.RegularSpeed;
    }

    private void Update()
    {
        //isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.Space) && canChangeSpeed)
        {
            FindObjectOfType<CustomCanvasSettings>().UpdatePlayPauseButtonImage();
            if (speedState == SpeedState.RegularSpeed/* || speedState == SpeedState.HighSpeed*/)
            {
                screenOff.SetActive(true); 
                foreach (HydraulicPress press in FindObjectsByType<HydraulicPress>(FindObjectsSortMode.None))
                {
                    press.StopBlockUpdate();
                }
                speedState = SpeedState.Paused;
                savedVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
            }
            else
            {
                foreach (HydraulicPress press in FindObjectsByType<HydraulicPress>(FindObjectsSortMode.None))
                {
                    press.ResumeBlockUpdate();
                }
                screenOff.SetActive(false);
                speedState = SpeedState.RegularSpeed;
                rb.velocity = savedVelocity;
            }
            anim.SetBool("isPaused", speedState == SpeedState.Paused);
        }
        /*else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (speedState == SpeedState.Paused)
                rb.velocity = savedVelocity;
            speedState = speedState == SpeedState.HighSpeed ? SpeedState.RegularSpeed : SpeedState.HighSpeed;
        }*/
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void FixedUpdate()
    {
        scaleSlider.value = transform.localScale.y;
        scaleText.text = $"{scaleSlider.value}";

        rb.isKinematic = speedState == SpeedState.Paused;

        if (canBeScaled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                //StartCoroutine(ScaleCharacter(true));
                ScaleCharacter(true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //StartCoroutine(ScaleCharacter(false));
                ScaleCharacter(false);
            }
        }

        if (IsCollidingHydraulicPress())
        {
            RestartLevel();
        }

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
                foreach (HydraulicPress press in FindObjectsByType<HydraulicPress>(FindObjectsSortMode.None))
                {
                    press.StopBlockUpdate();
                }
                savedVelocity = rb.velocity;
                rb.velocity = Vector2.zero;
                canChangeSpeed = false;
                break;
            case 1:
                foreach (HydraulicPress press in FindObjectsByType<HydraulicPress>(FindObjectsSortMode.None))
                {
                    press.ResumeBlockUpdate();
                }
                rb.velocity = savedVelocity;
                canChangeSpeed = false;
                break;
            case 2:
                rb.velocity = savedVelocity;
                canChangeSpeed = false;
                break;
            default:
                break;
        }
    }

    public void SwitchPlayPause()
    {
        FindObjectOfType<CustomCanvasSettings>().UpdatePlayPauseButtonImage();
        if (speedState == SpeedState.RegularSpeed/* || speedState == SpeedState.HighSpeed*/)
        {
            foreach (HydraulicPress press in FindObjectsByType<HydraulicPress>(FindObjectsSortMode.None))
            {
                press.StopBlockUpdate();
            }
            speedState = SpeedState.Paused;
            savedVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
        }
        else
        {
            foreach (HydraulicPress press in FindObjectsByType<HydraulicPress>(FindObjectsSortMode.None))
            {
                press.ResumeBlockUpdate();
            }
            speedState = SpeedState.RegularSpeed;
            rb.velocity = savedVelocity;
        }
        anim.SetBool("isPaused", speedState == SpeedState.Paused);
        screenOff.SetActive(speedState == SpeedState.Paused);
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DisableNoSpawnZoneBtn"))
        {
            foreach (var obj in GameObject.FindGameObjectsWithTag("NoSpawnZone"))
            {
                obj.SetActive(false);
            }
        }
        else if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (other.CompareTag("GameFinish"))
        {
            other.GetComponent<AudioSource>().Play();
            speedState = SpeedState.Paused;
            canChangeSpeed = false;
            yield return new WaitForSeconds(other.GetComponent<AudioSource>().clip.length);
            SceneManager.LoadScene(0);
        }
        yield return new WaitUntil(() => speedState != SpeedState.Paused);
        if (other != null)
        {
            if (other.CompareTag("NoScaleChangeZone"))
            {
                isCollidingNoScaleChangeZone = true;
            }
            else if (other.CompareTag("Jump Pad")/* && speedState != SpeedState.Paused*/)
            {
                other.GetComponent<Animator>().SetTrigger("JumpPad");
                rb.velocity = Vector2.up * jumpForce;
            }
            else if (other.CompareTag("Player Rotator")/* && speedState != SpeedState.Paused*/)
            {
                other.GetComponent<Animator>().SetTrigger("PlayerRotator");
                DOTween.PauseAll();
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                DOTween.PlayAll();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NoScaleChangeZone"))
        {
            isCollidingNoScaleChangeZone = false;
        }
    }

    bool IsCollidingRespawn()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(deathCheck.position, new Vector2(.1f, deathCheckHeight * (transform.localScale.y)), 0, groundLayer);
        print("IsCollidingRespawn() = " + (colliders.Length > 0));
        return colliders.Length > 0;
    }

    bool CanScaleUp()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(scaleUpPossibilityCheck.position, new Vector2(scaleCheckWidth * transform.localScale.x, .1f), 0, groundLayer);
        print("CanScaleUp() = " + (colliders.Length == 0));
        return colliders.Length == 0;
    }

    bool IsCollidingHydraulicPress()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(scaleUpPossibilityCheck.position, new Vector2(scaleCheckWidth * transform.localScale.x, .1f), 0, hydraulicPressLayer);
        print("IsCollidingHydraulicPress() = " + (colliders.Length == 0));
        return colliders.Length > 0;
    }

    bool CanScaleDown()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(scaleDownPossibilityCheck.position, new Vector2(scaleCheckWidth * transform.localScale.x, .1f), 0, groundLayer);
        print("CanScaleDown() = " + (colliders.Length == 0));
        return colliders.Length == 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        rb.isKinematic = true;
        rb.isKinematic = false;
        if (collision.collider.CompareTag("Antikostyl"))
        {
            print("5.0.1");
            return;
        }

        if (collision.collider.CompareTag("Respawn") || IsCollidingRespawn() || collision.collider.CompareTag("Kostyl"))
        {
            print("5.0.2");
            RestartLevel();
        }
    }

    /*private IEnumerator ScaleCharacter(bool positiveYScale)
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

        if ((newPlayerScale.y > 3.8f || newPlayerScale.y < 0.2f)/* && (newGravityScale > 1.6f || newGravityScale < 0.4f))
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
        //print($"SCALECHARACTER({newYScale}) CALLED");
        if (canBeScaled)
        {
            if (newYScale < 0.2f || newYScale > 3.8f)
            {
                return;
            }

            if (newYScale < transform.localScale.y)
            {
                print($"4.0.1");
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
            }
            else
            {
                print($"4.0.2");
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f);
            }
            if (newYScale > 2)
            {
                float scaleSubtracter = newYScale - 2;
                transform.localScale = new Vector2(transform.localScale.x > 0 ? 2 - scaleSubtracter : -2 + scaleSubtracter, newYScale);
            }
            else if (newYScale < 2)
            {
                print("4.1");
                float scaleSubtracter = 2 - newYScale;
                if (transform.localScale.x > 0)
                {
                    print("4.2.1");
                    transform.localScale = new Vector2(2 + scaleSubtracter, newYScale);
                }
                else
                {
                    print("4.2.2");
                    transform.localScale = new Vector2(-2 - scaleSubtracter, newYScale);
                }
            }
            else
            {
                transform.localScale = new Vector2(2, 2);
            }
        }
    }*/

    public void ScaleCharacter(bool increaseYAxis)
    {
        if ((!CanScaleUp() && !CanScaleDown()) || isCollidingNoScaleChangeZone) return;

        if (transform.localScale.y < 0.2f)
        {
            transform.localScale = new Vector2(3.8f, 0.2f);
            return;
        }
        else if (transform.localScale.y > 3.8f)
        {
            transform.localScale = new Vector2(0.2f, 3.8f);
            return;
        }

        Vector2 newPositiveYScale;
        Vector2 newNegativeYScale;

        if (transform.localScale.x > 0)
        {
            newPositiveYScale = new Vector2(transform.localScale.x - 0.04f, transform.localScale.y + 0.04f);
            newNegativeYScale = new Vector2(transform.localScale.x + 0.04f, transform.localScale.y - 0.04f);
        }
        else
        {
            newPositiveYScale = new Vector2(transform.localScale.x + 0.04f, transform.localScale.y + 0.04f);
            newNegativeYScale = new Vector2(transform.localScale.x - 0.04f, transform.localScale.y - 0.04f);
        }

        float newGravityScale = increaseYAxis == true ? rb.gravityScale - 0.024f : rb.gravityScale + 0.024f;

        Vector2 newPlayerScale = increaseYAxis == true ? newPositiveYScale : newNegativeYScale;

        if (newPlayerScale.y > 3.8f || newPlayerScale.y < 0.2f/* && (newGravityScale > 1.6f || newGravityScale < 0.4f)*/)
        {
            canBeScaled = true;
            return;
        }

        //rb.gravityScale = newGravityScale;

        if (newPlayerScale.y < transform.localScale.y || (!CanScaleUp() && CanScaleDown()))
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.04f);
        else if (newPlayerScale.y > transform.localScale.y || (CanScaleUp() && !CanScaleDown()))
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.04f);

        transform.localScale = newPlayerScale;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(deathCheck.position, new Vector2(.1f, deathCheckHeight * (transform.localScale.y)));
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(scaleUpPossibilityCheck.position, new Vector2(scaleCheckWidth * transform.localScale.x, .1f));
        Gizmos.DrawWireCube(scaleDownPossibilityCheck.position, new Vector2(scaleCheckWidth * transform.localScale.x, .1f));
    }
}
