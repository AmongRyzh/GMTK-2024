using UnityEngine;

public class BreakingBlock : MonoBehaviour
{
    [SerializeField] private float timeBreak = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Invoke("Breaking", timeBreak);
        }
    }

    private void Breaking()
    {
        Destroy(gameObject);
    }
}
