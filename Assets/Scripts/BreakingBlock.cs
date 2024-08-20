using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBlock : MonoBehaviour
{
    [SerializeField] private float timeBreak = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(Breaking());
        }
    }

    private IEnumerator Breaking()
    {
        yield return new WaitForSeconds(timeBreak);
        Destroy(gameObject);
    }
}
