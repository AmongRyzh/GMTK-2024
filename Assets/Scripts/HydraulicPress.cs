using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HydraulicPress : MonoBehaviour
{
    [SerializeField] GameObject press, chain;
    [SerializeField] float startPositionY;
    
    [SerializeField] float endPositionY;
    [SerializeField] float chainStartPositionY, chainStartScaleY;
    [SerializeField] float chainEndPositionY, chainEndScaleY;
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
        DOTween.Kill(press.transform);
        DOTween.Kill(chain.transform);
        StopCoroutine(blockUpdate);
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
            time1 = 0f;
            time2 = 1f - timeElapsed;
        }
        blockUpdate = StartCoroutine(BlockUpdate(true, time1, time2));
    }

    private IEnumerator BlockUpdate(bool resumed, float time1, float time2)
    {
        if (resumed)
        {
            isMovingDown = true;
            if (time1 != 0)
            {
                currentTween = press.transform.DOLocalMoveY(endPositionY, time1);
                chain.transform.DOLocalMoveY(chainEndPositionY, time1);
                chain.transform.DOScaleY(chainEndScaleY, time1);
            }
            print(7);
            yield return new WaitForSeconds(time1);
            isMovingDown = false;
            currentTween = press.transform.DOLocalMoveY(startPositionY, time2);
            chain.transform.DOLocalMoveY(chainStartPositionY, time2);
            chain.transform.DOScaleY(chainStartScaleY, time2);
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
            currentTween = press.transform.DOLocalMoveY(endPositionY, 0.2f);
            chain.transform.DOLocalMoveY(chainEndPositionY, 0.2f);
            chain.transform.DOScaleY(chainEndScaleY, 0.2f);
            print(7.4);
            yield return new WaitForSeconds(0.2f);
            isMovingDown = false;
            currentTween = press.transform.DOLocalMoveY(startPositionY, 1f);
            chain.transform.DOLocalMoveY(chainStartPositionY, 1f);
            chain.transform.DOScaleY(chainStartScaleY, 1f);
            print(7.5);
            yield return new WaitForSeconds(1f);
        }
    }

    public void MetalBlockCollision()
    {
        if (isMovingDown)
        {
            DOTween.Kill(press.transform);
            DOTween.Kill(chain.transform);
        }
    }
}
