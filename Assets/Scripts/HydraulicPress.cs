using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HydraulicPress : MonoBehaviour
{
    [SerializeField] GameObject press;
    [SerializeField] float startPositionY;
    [SerializeField] float endPositionY;
    bool isMovingDown;
    float timeElapsed;

    bool paused;

    Coroutine blockUpdate;

    Tween currentTween;

    private void Start()
    {
        blockUpdate = StartCoroutine(BlockUpdate(false, 0f, 0f));
    }

    public void StopBlockUpdate()
    {
        timeElapsed = currentTween.Elapsed();
        DOTween.Kill(transform);
        StopCoroutine(FindObjectOfType<HydraulicPress>().blockUpdate);
    }

    public void ResumeBlockUpdate()
    {
        float time1;
        float time2;
        if (isMovingDown)
        {
            time1 = 0.2f - timeElapsed;
            time2 = 1f;
        }
        else
        {
            time1 = 0.2f;
            time2 = 1f - timeElapsed;
        }
        blockUpdate = StartCoroutine(BlockUpdate(true, time1, time2));
    }

    private IEnumerator BlockUpdate(bool resumed, float time1, float time2)
    {
        if (resumed)
        {
            isMovingDown = true;
            currentTween = press.transform.DOMoveY(endPositionY, time1);
            print(7);
            yield return new WaitForSeconds(time1);
            isMovingDown = false;
            currentTween = press.transform.DOMoveY(startPositionY, time2);
            print(7.1);
            yield return new WaitForSeconds(time2);
            print(7.2);
        }

        /*if (paused)
        {
            print(7.3);
            yield break;
        }*/

        while (true)
        {
            isMovingDown = true;
            currentTween = press.transform.DOMoveY(endPositionY, 0.2f);
            print(7.4);
            yield return new WaitForSeconds(0.2f);
            isMovingDown = false;
            currentTween = press.transform.DOMoveY(startPositionY, 1f);
            print(7.5);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MetalBlock") && isMovingDown)
        {
            DOTween.Kill(transform);
        }
    }
}
