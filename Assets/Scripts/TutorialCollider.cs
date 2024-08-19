using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    [SerializeField] int colliderID;
    bool collided;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collided)
        {
            collided = true;
            FindObjectOfType<Tutorial>().TutorialCollider(colliderID);
        }
    }
}
